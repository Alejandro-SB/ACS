using ACS.Core.Entities;
using ACS.Core.Interfaces.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.Core.QueryFilter
{
    public class UserQueryFilter : QueryFilter<ACSUser>
    {
        public UserQueryFilter(int identityId) : base(x => x.IdentityId == identityId)
        {
            Include(u => u.RefreshTokens);
        }
    }
}