using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Model;
using SharedData;

namespace Auth
{
    public class DataSeed
    {
        public static async Task SeedDataAsync(DataContext context, UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<ApplicationUser>
                {
                    new()
                    {
                        DisplayName = "TestUserFirst",
                        UserName = "TestUserFirst",
                        Email = "testuserfirst@test.com"
                    },

                    new()
                    {
                        DisplayName = "TestUserSecond",
                        UserName = "TestUserSecond",
                        Email = "testusersecond@test.com"
                    }
                };

                foreach (var user in users) await userManager.CreateAsync(user, "qazwsX123@");
            }
        }
    }
}