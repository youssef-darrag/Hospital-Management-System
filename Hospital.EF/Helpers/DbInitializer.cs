using Hospital.Core.Helpers;
using Hospital.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hospital.EF.Helpers
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {

                throw;
            }

            if (!_roleManager.RoleExistsAsync(WebSiteRoles.Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.Doctor)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.Patient)).GetAwaiter().GetResult();

                var result = _userManager.CreateAsync(new ApplicationUser
                {
                    Name = "Youssef Darrag",
                    UserName = "YoussefDarrag",
                    Email = "darrag@gmail.com"

                }, "P@ssword123").GetAwaiter().GetResult();

                var user = _context.ApplicationUsers.FirstOrDefault(u => u.Email == "darrag@gmail.com");

                if (user != null)
                {
                    _userManager.AddToRoleAsync(user, WebSiteRoles.Admin).GetAwaiter().GetResult();
                }
            }
        }
    }
}
