using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACS.API.Authentication
{
    public static class ApiClaims
    {
        public static class ClaimIdentifiers
        {
            public const string Rol = "rol";
            public const string Id = "id";
        }

        public static class ClaimValues
        {
            public const string ApiAccess = "api_access";
        }
    }
}