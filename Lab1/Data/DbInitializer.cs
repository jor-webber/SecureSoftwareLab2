using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Lab1.Models;

namespace Lab1.Data
{
    /// <summary>
    /// DbInitializer class used to seed data into the database.
    /// </summary>
    public static class DbInitializer
    {
        public static AppSecrets AppSecrets { get; set; }

        /// <summary>
        /// Seeds the Users and Roles into the database
        /// </summary>
        /// <param name="serviceProvider">Service Provider for application</param>
        /// <returns>Number based on if parts fail</returns>
        public static async Task<int> SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            // create the database if it doesn't exist
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Check if roles already exist and exit if there are
            if (roleManager.Roles.Count() > 0)
                return 1;  // should log an error message here
            // Seed roles
            int result = await SeedRoles(roleManager);
            if (result != 0)
                return 2;  // should log an error message here
            // Check if users already exist and exit if there are
            if (userManager.Users.Count() > 0)
                return 3;  // should log an error message here
            // Seed users
            result = await SeedUsers(userManager);
            if (result != 0)
                return 4;  // should log an error message here
            return 0;
        }

        /// <summary>
        /// Seeds the Roles table in the database.
        /// </summary>
        /// <param name="roleManager">The role manager</param>
        /// <returns>Number based on if task fails or succeeds</returns>
        private static async Task<int> SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            // Create Player Role
            var result = await roleManager.CreateAsync(new IdentityRole("Player"));
            if (!result.Succeeded)
                return 1;

            // Create Manager Role
            result = await roleManager.CreateAsync(new IdentityRole("Manager"));
            if (!result.Succeeded)
                return 2; 
            return 0;
        }

        /// <summary>
        /// Seeds the Users table in the database.
        /// </summary>
        /// <param name="userManager">The user manager</param>
        /// <returns>Number based on if task fails or succeeds</returns>
        private static async Task<int> SeedUsers(UserManager<ApplicationUser> userManager)
        {
            // Create Manager User
            var adminUser = new ApplicationUser
            {
                UserName = "the.manager@mohawkcollege.ca",
                Email = "the.manager@mohawkcollege.ca",
                FirstName = "The",
                LastName = "Manager",
                EmailConfirmed = true,
                BirthDate = new DateOnly(1988, 3, 12).ToString()
            };
            var result = await userManager.CreateAsync(adminUser, AppSecrets.ManagerPassword);
            if (!result.Succeeded)
                return 1;  

            // Assign user to Manager role
            result = await userManager.AddToRoleAsync(adminUser, "Manager");
            if (!result.Succeeded)
                return 2;  

            // Create Player User
            var memberUser = new ApplicationUser
            {
                UserName = "the.player@mohawkcollege.ca",
                Email = "the.player@mohawkcollege.ca",
                FirstName = "The",
                LastName = "Player",
                EmailConfirmed = true,
                BirthDate = new DateOnly(1994, 5, 21).ToString()
            };

            result = await userManager.CreateAsync(memberUser, AppSecrets.PlayerPassword);
            if (!result.Succeeded)
                return 3; 

            // Assign user to Player role
            result = await userManager.AddToRoleAsync(memberUser, "Player");
            if (!result.Succeeded)
                return 4; 

            return 0;
        }
    }
}