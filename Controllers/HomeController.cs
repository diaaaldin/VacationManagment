using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VacationManagment.Data;
using VacationManagment.Models;

namespace VacationManagment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VacationDbContext _context;

        public HomeController(ILogger<HomeController> logger , VacationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Department = _context.Departments.Count();
            ViewBag.Employee = _context.Employees.Count();
            ViewBag.VacationType = _context.VacationTypes.Count();
            ViewBag.Request = _context.RequestVacations.Count();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}