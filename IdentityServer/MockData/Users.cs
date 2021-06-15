using System;
using System.Collections.Generic;
using IdentityServer4.Models;

namespace IdentityServer.MockData
{
    public static class Users
    {
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientName = "Client Application1",
                    ClientId = "t8agr5xKt4$3",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("eb300de4-add9-42f4-a3ac-abd3c60f1919".Sha256(),DateTime.Today.AddDays(100)) },
                    AllowedScopes = new List<string> { "api.orders","api.arts" }
                }
            };
    }
}