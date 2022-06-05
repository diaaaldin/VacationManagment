using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacationManagment.Data;
using VacationManagment.Models;

namespace VacationManagment.Controllers
{
    [Authorize(Policy = "Admin Policy")]
    public class EmployeeController : Controller
    {
        private readonly VacationDbContext _context;

        public EmployeeController(VacationDbContext context)
        {
            _context = context;
        }

        public IActionResult Employees()
        {
             
            return View(_context.Employees.Include(x=> x.Department).OrderBy(x=>x.Id).ToList());
        }

        public IActionResult Create()
        {
            ViewBag.DepartmentId = _context.Departments.OrderBy(x=>x.Name).ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee model)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Employees));
            }
            ViewBag.DepartmentId = _context.Departments.OrderBy(x => x.Name).ToList();
            return View(model);
        }
        public IActionResult Edit(int id)
        {
            var chick = _context.Employees.FirstOrDefault(x => x.Id == id);
            if (chick == null)
            {
                return NotFound();
            }
            ViewBag.DepartmentId1 = _context.Departments.OrderBy(x => x.Name).ToList();
            return View(chick);
        }
        [HttpPost]
        public IActionResult Edit(Employee model)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Update(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Employees));
            }
            ViewBag.DepartmentId1 = _context.Departments.OrderBy(x => x.Name).ToList();
            return View();
        }

        public IActionResult Delete(int id)
        {
            var Chick = _context.Employees.FirstOrDefault(x => x.Id == id);
            if (Chick == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(Chick);
            _context.SaveChanges();
            return RedirectToAction(nameof(Employees));
        }

    }
}
