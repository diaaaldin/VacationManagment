using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VacationManagment.Data;
using VacationManagment.Services;
using VacationManagment.ViewModel;

namespace VacationManagment.Controllers
{
    [Authorize(Policy = "Admin Policy")]
    public class RolesController : Controller
    {
         readonly IManageRolesService _manageRolesService;
         readonly IManageUsersService _manageUsersService;
        private readonly VacationDbContext _context;

        public RolesController(IManageRolesService manageRolesService,IManageUsersService manageUsersService , VacationDbContext context )
        {
            _manageRolesService = manageRolesService;
            _manageUsersService = manageUsersService;
            _context = context;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            return View(await _manageRolesService.GetRoles());
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roleViewModel = await _manageRolesService.GetRole(id);
            if (roleViewModel == null)
            {
                return NotFound();
            }

            return View(roleViewModel);
        }

        // GET: Roles/Create
        public async Task<IActionResult> Create()
        {
            //var employee = _context.Employees.Select(x => new { Id = x.Id, Name = x.EmpName }).ToList();
            //ViewBag.employeeList = new SelectList(employee, "Id", "Name", id /*employee.FirstOrDefault().Id*/ );
            //var roles = (await _manageRolesService.GetRoles()).Select(x => x.Name).ToList();

            //var Claims = (await _manageRolesService.GetRoles()).Select(x => x.Claims);
            //ViewBag.claims = Claims;
           
            return View();

        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Claims")] RoleViewModel roleViewModel)
        {
            //if (ModelState.IsValid)
            //{
                await _manageRolesService.CreateRole(roleViewModel);
                return RedirectToAction(nameof(Index));
            //}
            //return View(roleViewModel);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roleViewModel = await _manageRolesService.GetRole(id);
            if (roleViewModel == null)
            {
                return NotFound();
            }
            return View(roleViewModel);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Claims")] RoleViewModel roleViewModel)
        {
            if (id != roleViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                await _manageRolesService.UpdateRole(id, roleViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(roleViewModel);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roleViewModel = await _manageRolesService.GetRole(id);
            if (roleViewModel == null)
            {
                return NotFound();
            }

            return View(roleViewModel);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _manageRolesService.DeleteRole(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
