using System;
using System.Collections.Generic;
using IdentityServer4.Models;

namespace IdentityServer.MockData
{
    public static class Resources
    {
        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource {
                    Name = "api.stats",
                    DisplayName = "api.stats",
                    ApiSecrets = { new Secret("Y2F0Y2hlciUyMHdvbmclMjBsb3ZlJTIwLm5ldA==".Sha256(),DateTime.Now.AddDays(100)) },
                    Scopes = new List<string> {
                        "api.stats"
                    }
                },
                
                new ApiResource {
                    Name = "api.orders",
                    DisplayName = "api.orders",
                    ApiSecrets = { new Secret("Y2F0Y2hlciUyMHdvbmclMjBsb3ZlJTIwLm5ldA==".Sha256()) },
                    Scopes = new List<string> {
                       
                        new string("api.orders"),
                        
                    }
                },
                
                new ApiResource {
                    Name = "api.arts",
                    DisplayName = "api.arts",
                    ApiSecrets = { new Secret("Y2F0Y2hlciUyMHdvbmclMjBsb3ZlJTIwLm5ldA==".Sha256()) },
                    Scopes = new List<string> {
                       
                        new string("api.arts"),
                        new string("api.orders"),
                        
                    }
                },
                
                new ApiResource {
                    Name = "api.artistTest",
                    DisplayName = "api.artistTest",
                    ApiSecrets = { new Secret("Y2F0Y2hlciUyMHdvbmclMjBsb3ZlJTIwLm5ldA==".Sha256()) },
                    Scopes = new List<string> {
                       
                        new string("api.artistTest"),
                        
                    }
                },
                
                new ApiResource {
                    Name = "api.auth",
                    DisplayName = "api.auth",
                    ApiSecrets = { new Secret("Y2F0Y2hlciUyMHdvbmclMjBsb3ZlJTIwLm5ldA==".Sha256()) },
                    Scopes = new List<string> {
                       
                        new string("api.auth"),
                        
                    }
                },
             
            };
    }
}