using ACS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Core.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<ACSUser>
    {
        Task<ACSUser?> FindByNameAsync(string userName);
        Task<bool> CheckPasswordAsync(ACSUser user, string password);
    }
}