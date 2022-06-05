using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationManagment.ViewModel;

namespace VacationManagment.Services
{
    public interface IManageUsersService
    {
        Task<UserViewModel> GetUser(string userId);
        Task<List<UserViewModel>> GetUsers();
        Task AssignRoles(UserViewModel role);
    }

    public class ManageUsersService : IManageUsersService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IManageRolesService _manageRolesService;

        public ManageUsersService(UserManager<IdentityUser> userManager, IManageRolesService manageRolesService)
        {
            _userManager = userManager;
            _manageRolesService = manageRolesService;
        }

        public async Task<UserViewModel> GetUser(string userId)
        {
            var userEntity = await _userManager.FindByIdAsync(userId);
            if (userEntity == null)
                throw new Exception("User does not exist");

            var roles = await _userManager.GetRolesAsync(userEntity);
            var roleViewModel = new UserViewModel()
            {
                Id = userId,
                UserName = userEntity.UserName,
                Roles = roles.ToList(),
            };
            return roleViewModel;
        }

        public async Task<List<UserViewModel>> GetUsers()
        {
            var users = _userManager.Users.AsEnumerable();
            var usersViewModels = new List<UserViewModel>();
            foreach (var item in users)
            {
                usersViewModels.Add(new UserViewModel()
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    Roles = (await _userManager.GetRolesAsync(item)).ToList(),
                });
            }

            return usersViewModels;
        }

        public async Task AssignRoles(UserViewModel user)
        {
            var userEntity = await _userManager.FindByIdAsync(user.Id);
            if (userEntity == null)
                throw new Exception("User does not exist");

            var userRoles = await _userManager.GetRolesAsync(userEntity);
            if (user.Roles == null || user.Roles.Count == 0)
            {
                await _userManager.RemoveFromRolesAsync(userEntity, userRoles);
            }
            else
            {
                var rolesToAdd = user.Roles.Where(x => !userRoles.Any(ur => ur == x));
                var rolesToRemove = userRoles.Where(ur => !user.Roles.Any(x => ur == x));

                await _userManager.AddToRolesAsync(userEntity, rolesToAdd);
                await _userManager.RemoveFromRolesAsync(userEntity, rolesToRemove);
            }
        }
    }
}
