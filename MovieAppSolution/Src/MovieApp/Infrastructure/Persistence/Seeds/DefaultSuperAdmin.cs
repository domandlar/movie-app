using Microsoft.AspNetCore.Identity;
using MovieApp.Domain.Entities;
using MovieApp.Domain.Enums;

namespace MovieApp.Infrastructure.Persistence.Seeds
{
    public static class DefaultSuperAdmin
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new User
            {
                UserName = "superadmin",
                Email = "superadmin@movieapp.com",
                FirstName = "Super",
                LastName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "P4$$w0rd!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                }
            }
        }
    }
}
