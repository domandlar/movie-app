using Microsoft.AspNetCore.Identity;
using MovieApp.Domain.Entities;
using MovieApp.Domain.Enums;

namespace MovieApp.Infrastructure.Persistence.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Director.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Actor.ToString()));
        }
    }
}
