using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VacationManagment.Data;
using VacationManagment.ViewModel;

namespace VacationManagment.Controllers
{
    [Authorize(Policy = "Admin Policy")]
    public class ReportController : Controller
    {

        private readonly VacationDbContext _context;
        public ReportController(VacationDbContext context)
        {
            _context = context;
        }

        public  IActionResult Index(int? id)
        {
            var Employee = _context.RequestVacations.Include(x => x.Employee).Where(x => x.EmployeeId == id);

           
            int VacationBalance1 = 0;
            int TotalVacation1 = 0;
          

            foreach (var item in Employee)
            {
                VacationBalance1 = item.Employee.VacationBalance;
                TotalVacation1 += ((TimeSpan)(item.EndDate - item.StartDate)).Days+1;
                
            }
            int Total1 = VacationBalance1 - TotalVacation1 ;

            var EmployeeReport = _context.RequestVacations.Include(x => x.Employee).Where(x => x.EmployeeId == id).Select(x => new EmployeesReportViewModel
            {
                EmpId = x.EmployeeId,
                EmployeeName = x.Employee.Name,
                VacationBalance = VacationBalance1,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                TotalVacation = TotalVacation1,
                Total = Total1
            });
            var employee = _context.Employees.Select(x => new {Id = x.Id , Name = x.Name}).ToList();
            ViewBag.Employee = new SelectList(employee, "Id", "Name", id);
            return View(EmployeeReport);
        }

    }
}
