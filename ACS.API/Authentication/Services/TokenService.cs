using ACS.Core.Extensions;
using ACS.Core.Interfaces;
using ACS.Core.UseCases.Authentication.Dto;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ACS.API.Authentication.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _options;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

        public TokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public AccessToken GenerateEncodedToken(int identityId, string userName, string refreshToken)
        {
            var identity = GenerateClaimsIdentity(identityId, userName);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUnixDate().ToString()),
                identity.FindFirst("rol"),
                identity.FindFirst("id")
            };

            var jwt = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                _options.NotBefore,
                _options.Expiration,
                _options.SigningCredentials
                );

            return new AccessToken(_tokenHandler.WriteToken(jwt), (int)_options.ValidFor.TotalSeconds, refreshToken);
        }

        public string GenerateToken(int size = 32)
        {
            var num = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(num);

            return Convert.ToBase64String(num);
        }

        public ClaimsPrincipal? GetPrincipalFromToken(string token, string key)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateLifetime = false
                };

                var principal = _tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                if (!(validatedToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch //TODO: log exception
            {
                return null;
            }
        }

        private ClaimsIdentity GenerateClaimsIdentity(int identityId, string userName)
        {
            var identity = new GenericIdentity(userName, "Token");

            //TODO: Review claim generation
            var claims = new[]
            {
                new Claim("id", identityId.ToString()),
                new Claim("rol", "user")
            };

            return new ClaimsIdentity(identity, claims);
        }
    }
}
