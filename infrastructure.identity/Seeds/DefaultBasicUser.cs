using core.application.Enums;
using infrastructure.identity.Models;
using Microsoft.AspNetCore.Identity;

namespace infrastructure.identity.Seeds
{
    public class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "usernamebasic",
                Email = "emailbasic@mail.com",
                Nombre = "nombrebasic",
                Apellido = "apellidobasic",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);

                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Pa$$word123");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                }
            }
        }
    }
}
