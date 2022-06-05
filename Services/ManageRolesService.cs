using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VacationManagment.ViewModel;

namespace VacationManagment.Services
{
    public interface IManageRolesService
    {
        Task<RoleViewModel> GetRole(string roleId);
        Task<List<RoleViewModel>> GetRoles();
        Task<RoleViewModel> CreateRole(RoleViewModel role);
        Task<RoleViewModel> UpdateRole(string roleId, RoleViewModel role);
        Task DeleteRole(string roleId);
    }

    public class ManageRolesService : IManageRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageRolesService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<RoleViewModel> GetRole(string roleId)
        {
            var roleEntity = await _roleManager.FindByIdAsync(roleId);
            if (roleEntity == null)
                throw new Exception("Role does not exist");

            var claims = await _roleManager.GetClaimsAsync(roleEntity);
            var roleViewModel = new RoleViewModel()
            {
                Id = roleId,
                Name = roleEntity.Name,
                Claims = claims.Select(x => x.Type).ToList(),
            };
            return roleViewModel;
        }

        public async Task<List<RoleViewModel>> GetRoles()
        {
            var roles = _roleManager.Roles.AsEnumerable();
            var rolesViewModels = new List<RoleViewModel>();
            foreach (var item in roles)
            {
                rolesViewModels.Add(new RoleViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Claims = (await _roleManager.GetClaimsAsync(item)).Select(x => x.Type).ToList(),
                });
            }

            return rolesViewModels;
        }

        public async Task<RoleViewModel> CreateRole(RoleViewModel role)
        {
            var roleEntity = new IdentityRole(role.Name);
            if (await _roleManager.RoleExistsAsync(role.Name))
                throw new Exception("Role already exist");

            await _roleManager.CreateAsync(roleEntity);
            foreach (var item in role.Claims)
            {
                await _roleManager.AddClaimAsync(roleEntity, new Claim(item, ""));
            }

            return role;
        }

        public async Task<RoleViewModel> UpdateRole(string roleId, RoleViewModel role)
        {
            var roleEntity = await _roleManager.FindByIdAsync(roleId);
            if (roleEntity == null)
                throw new Exception("Role does not exist");

            roleEntity.Name = role.Name;
            await _roleManager.UpdateAsync(roleEntity);

            var roleClaims = await _roleManager.GetClaimsAsync(roleEntity);
            var claimsToAdd = role.Claims.Where(x => !roleClaims.Any(rc => rc.Type == x));
            var claimsToRemove = roleClaims.Where(rc => !role.Claims.Any(x => rc.Type == x));
            foreach (var item in claimsToAdd)
            {
                await _roleManager.AddClaimAsync(roleEntity, new Claim(item, ""));
            }
            foreach (var item in claimsToRemove)
            {
                await _roleManager.RemoveClaimAsync(roleEntity, item);
            }

            return role;
        }

        public async Task DeleteRole(string roleId)
        {
            var roleEntity = await _roleManager.FindByIdAsync(roleId);
            if (roleEntity == null)
                throw new Exception("Role does not exist");

            await _roleManager.DeleteAsync(roleEntity);
        }
    }
}
