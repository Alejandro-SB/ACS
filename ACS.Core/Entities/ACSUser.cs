#nullable disable //Disable nullability for this file. Entities must map into DB correctly. Maybe configurable through Fluent API?

using System.Linq;
using ACS.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;
using ACS.Core.Extensions;

namespace ACS.Core.Entities
{
    public class ACSUser : BaseEntity
    {
        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int IdentityId { get; set; }

        public ACSUser() { }

        public ACSUser(string firstName, string secondName, string lastName, string userName)
        {
            FirstName = firstName;
            SecondName = secondName;
            LastName = lastName;
            UserName = userName;
        }

        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        public void AddRefreshToken(string token, int userId, string ip, double daysToExpire=3)
        {
            _refreshTokens.Add(new RefreshToken(token, userId, DateTime.UtcNow.AddDays(daysToExpire), ip));
        }

        public bool HasValidRefreshToken(string token)
        {
            return _refreshTokens.Any(t => t.Token == token && t.IsActive);
        }

        public bool RemoveRefreshToken(string token)
        {
            return _refreshTokens.RemoveFirst(x => x.Token == token);
        }
    }
}
