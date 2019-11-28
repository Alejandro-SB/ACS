using ACS.Core.Entities;
using ACS.Core.Interfaces.Repositories;
using ACS.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ACS.Infrastructure.Data.Repositories
{
    public class UserRepository : Repository<ACSUser>, IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(UserManager<AppUser> userManager, ACSDbContext dbContext) : base(dbContext)
        {
            _userManager = userManager;
        }

        public async Task<bool> CheckPasswordAsync(ACSUser user, string password)
        {
            var appUser = _userManager.Users.FirstOrDefault(u => u.Id == user.IdentityId);
            return await _userManager.CheckPasswordAsync(appUser, password);
        }

        public async Task<ACSUser?> FindByNameAsync(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);
            return appUser == null ? null : await FirstOrDefault(x => x.IdentityId == appUser.Id);
        }
    }
}
