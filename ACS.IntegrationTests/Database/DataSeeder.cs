using ACS.Core.Entities;
using ACS.Infrastructure.Data;
using ACS.Infrastructure.Data.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.IntegrationTests.Database
{
    public static class DataSeeder
    {
        public static void Populate(ACSDbContext dbContext)
        {
            dbContext.Users.Add(new AppUser
            {
                Id = 1,
                UserName = "acstestuser",
                NormalizedUserName = "ACSTESTUSER",
                Email = "acsuser@acs.es",
                NormalizedEmail = "ACSUSER@ACS.ES",
                PasswordHash = "AQAAAAEAACcQAAAAEKQBX+Qqr/M3qmEcoM3Y/M/8XtVKZ9RnaiXlamue6MIuhOoYONHS7BUHkmOxpF/X3w==",
                SecurityStamp = "YIJZLWUFIIDD3IZSFDD7OQWG6D4QIYPB",
                ConcurrencyStamp = "e432007d-0a54-4332-9212-ca9d7e757275"
            });

            var user = new ACSUser("A", "C", "S", "acstestuser");
            user.IdentityId = 1;
            user.AddRefreshToken("sf+BoDDXriyqkpwB4Wl3I59AYecTs9j37a981ijAGXs=", 1, "127.0.0.1");
            dbContext.ACSUsers!.Add(user);
            dbContext.SaveChanges();
        }
    }
}
