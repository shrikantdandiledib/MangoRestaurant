﻿using IdentityModel;
using Manago.Services.Identity.DbContexts;
using Manago.Services.Identity.Helper;
using Manago.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Manago.Services.Identity.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContexts _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContexts db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            if (_roleManager.FindByNameAsync(ClientRoles.Admin).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(ClientRoles.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(ClientRoles.Customer)).GetAwaiter().GetResult();
            }
            else { return; }

            ApplicationUser adminUser = new ApplicationUser()
            {
                UserName = "admin1@gmail.com",
                Email = "admin1@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "111111111111",
                FirstName = "Ben",
                LastName = "Admin"
            };

            _userManager.CreateAsync(adminUser, "Admin@123").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(adminUser, ClientRoles.Admin).GetAwaiter().GetResult();

            var temp1 = _userManager.AddClaimsAsync(adminUser, new Claim[] {
                new Claim(JwtClaimTypes.Name,adminUser.FirstName+" "+ adminUser.LastName),
                new Claim(JwtClaimTypes.GivenName,adminUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName,adminUser.LastName),
                new Claim(JwtClaimTypes.Role,ClientRoles.Admin),
            }).Result;

            ApplicationUser customerUser = new ApplicationUser()
            {
                UserName = "customer1@gmail.com",
                Email = "customer1@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "111111111111",
                FirstName = "Ben",
                LastName = "Cust"
            };

            _userManager.CreateAsync(customerUser, "Admin123*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(customerUser, ClientRoles.Customer).GetAwaiter().GetResult();

            var temp2 = _userManager.AddClaimsAsync(customerUser, new Claim[] {
                new Claim(JwtClaimTypes.Name,customerUser.FirstName+" "+ customerUser.LastName),
                new Claim(JwtClaimTypes.GivenName,customerUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName,customerUser.LastName),
                new Claim(JwtClaimTypes.Role,ClientRoles.Customer),
            }).Result;
        }

       
    }
}
