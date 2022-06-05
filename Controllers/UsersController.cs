using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VacationManagment.Services;
using VacationManagment.ViewModel;

namespace VacationManagment.Controllers
{
    [Authorize(Policy = "Admin Policy")]
    public class UsersController : Controller
    {
        private readonly IManageUsersService _manageUsersService;
        private readonly IManageRolesService _manageRolesService;

        public UsersController(IManageUsersService manageUsersService, IManageRolesService manageRolesService)
        {
            _manageUsersService = manageUsersService;
            _manageRolesService = manageRolesService;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await _manageUsersService.GetUsers();
            return View(users);
        }

        // GET: Users/AssignRole
        public async Task<IActionResult> AssignRoles(string id)
        {
            var user = await _manageUsersService.GetUser(id);
            var roles = (await _manageRolesService.GetRoles()).Select(x => x.Name).ToList();
            ViewBag.Roles = roles;
            return View(user);
        }

        // POST: Users/AssignRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRoles([Bind("Id,UserName,Roles")] UserViewModel userViewModel)
        {
            await _manageUsersService.AssignRoles(userViewModel);
            return RedirectToAction(nameof(Index));
        }
    }
}
