using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using VacationManagment.Data;
using VacationManagment.Models;
using VacationManagment.ViewModel;

namespace VacationManagment.Controllers
{
    [Authorize(Policy = "User Policy")]
    public class VacationPlanController : Controller
    {
        private readonly VacationDbContext _context;
        private readonly INotyfService _notyf;

        public VacationPlanController(VacationDbContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            return View(_context.RequestVacations
                .Include(x => x.Employee)
                .Include(x=>x.VacationType)
                .OrderByDescending(x=> x.RequestDate)
                .ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }
        //method 2 how you get your information without reloud
        public IActionResult GetVacationTypes()
        {

            return Json(_context.VacationTypes.OrderBy(x=>x.Id).ToList());
        }
     

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VacationPlan model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (_context.RequestVacations.Any(x => x.EmployeeId == model.RequestVacation.EmployeeId && x.StartDate == model.RequestVacation.StartDate))
            {
                ModelState.AddModelError("RequestVacation", "This Employee has a vacation in this day");
            }

            var Result = new RequestVacation
            {
                EmployeeId = model.RequestVacation.EmployeeId,
                VacationTypeId = model.RequestVacation.VacationTypeId,
                StartDate = model.RequestVacation.StartDate,
                EndDate = model.RequestVacation.EndDate,
                Comment = model.RequestVacation.Comment,
                Approved = false,
                RequestDate = DateTime.Now
            };
            _context.RequestVacations.Add(Result);
            _notyf.Success("Add Success");
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var chick = _context.RequestVacations
                .Include(x => x.Employee)
                .Include(x => x.VacationType)
                .Include(x => x.VacationPlanList)
                .FirstOrDefault(x => x.Id == id);

            var Employee = _context.Employees.Select(x => new { id = x.Id, name = x.Name }).ToList();
            ViewBag.employee = new SelectList(Employee, "id", "name", id);
            var Vacation = _context.VacationTypes.Select(x => new { id = x.Id, name = x.VacationName }).ToList();
            ViewBag.Vacation = new SelectList(Vacation, "id", "name", id);

            var model = new RequestVacation
            {
                EmployeeId = chick.EmployeeId,
                VacationTypeId = chick.VacationTypeId,
                StartDate = chick.StartDate,
                EndDate = chick.EndDate,
                Comment = chick.Comment,
                Approved = chick.Approved,

            };

            return View(model);

        }
        [HttpPost]
        public IActionResult Edit(RequestVacation model)
        {
            if (model == null)
            {
                return View(model);
            }
            
            _context.RequestVacations.Update(model);
            _notyf.Success("Edit Success");
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public IActionResult Delete(int id)
        {
            var chick = _context.RequestVacations.FirstOrDefault();
            if (chick != null)
            {
                _context.RequestVacations.Remove(chick);
                _notyf.Error("Delete Item");
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
