#nullable disable
using ACS.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.Core.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public int UserId { get; private set; }
        public bool IsActive => DateTime.UtcNow <= ExpirationDate;
        public string Ip { get; private set; }

        public RefreshToken() { }
        public RefreshToken(string token, int userId, DateTime expirationDate, string ip)
        {
            UserId = userId;
            Token = token;
            ExpirationDate = expirationDate;
            Ip = ip;
        }
    }
}