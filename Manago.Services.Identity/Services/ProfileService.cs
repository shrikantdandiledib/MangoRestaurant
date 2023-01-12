using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Manago.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Manago.Services.Identity.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly UserManager<ApplicationUser> _userMgr;
        private readonly RoleManager<IdentityRole> _roleMgr;
        public ProfileService(UserManager<ApplicationUser> userMgr, RoleManager<IdentityRole> roleMgr, IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory)
        {
            _userMgr = userMgr;
            _roleMgr = roleMgr;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string subject = context.Subject.GetSubjectId();
            ApplicationUser user = await _userMgr.FindByIdAsync(subject);
            ClaimsPrincipal userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

            List<Claim> claims = userClaims.Claims.ToList();
            claims = claims.Where(cl => context.RequestedClaimTypes.Contains(cl.Type)).ToList();
            if (_userMgr.SupportsUserRole)
            {
                IList<string> roles = await _userMgr.GetRolesAsync(user);
                // Added Custom Claims 
                claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
                claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
                foreach (var roleName in roles)
                {
                    claims.Add(new Claim(JwtClaimTypes.Role, roleName));
                    #region If We have Claims with Role we can implement them dynamically
                    if (_roleMgr.SupportsRoleClaims)
                    {
                        IdentityRole role = await _roleMgr.FindByNameAsync(roleName);
                        if (role != null)
                        {
                            claims.AddRange(await _roleMgr.GetClaimsAsync(role));
                        }
                    }
                    #endregion
                }
            }
            context.IssuedClaims = claims;

        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            string sub = context.Subject.GetSubjectId();
            ApplicationUser user = await _userMgr.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
