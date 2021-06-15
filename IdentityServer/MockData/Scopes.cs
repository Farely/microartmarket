using System.Collections.Generic;
using IdentityServer4.Models;

namespace IdentityServer.MockData
{
    public static class Scopes
    {
        public static IEnumerable<ApiScope> StartScopes =>
            new List<ApiScope>
            {
                new ApiScope("api.stats"),
                new ApiScope("api.orders"),
                new ApiScope("api.arts"),
                new ApiScope("api.artistTest"),
                new ApiScope("api.auth"),
            };
    }
}