using ACS.Core.UseCases.Authentication.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ACS.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(int size = 32);
        AccessToken GenerateEncodedToken(int identityId, string userName, string refreshToken);
        ClaimsPrincipal? GetPrincipalFromToken(string token, string key);
    }
}