using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacationManagment.Data;
using VacationManagment.Models;

namespace VacationManagment.Controllers
{
    [Authorize(Policy = "Admin Policy")]
    public class DepartmentController : Controller
    {
        private readonly VacationDbContext _context;

        public DepartmentController(VacationDbContext context)

        {
            _context = context;
        }
        public IActionResult Departments()
        {
            return View(_context.Departments.OrderBy(x=>x.Name).ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department model)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Departments));
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var result = _context.Departments.FirstOrDefault(x => x.Id == id);
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department model)
        {

            if (ModelState.IsValid)
            {
                _context.Departments.Update(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Departments));
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var Chick = _context.Departments.FirstOrDefault(x => x.Id == id);
            if (Chick == null)
            {
                return NotFound();
            }
            _context.Departments.Remove(Chick);
            _context.SaveChanges();
            return RedirectToAction(nameof(Departments));
        }
      
    }
}
