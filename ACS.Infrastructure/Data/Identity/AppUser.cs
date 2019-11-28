using ACS.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.Infrastructure.Data.Identity
{
    public sealed class AppUser : IdentityUser<int>
    {
        public ACSUser ToUser()
        {
            return new ACSUser()
            {
                IdentityId = this.Id,
                PasswordHash = this.PasswordHash,
                Email = this.Email
            };
        }

        public static AppUser FromUser(ACSUser user)
        {
            return new AppUser()
            {
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }
}