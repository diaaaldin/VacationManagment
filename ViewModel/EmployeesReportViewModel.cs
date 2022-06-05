using System.ComponentModel.DataAnnotations;

namespace VacationManagment.ViewModel
{
    public class EmployeesReportViewModel
    {
        public int EmpId { get; set; }


        [Display(Name = "Name")]
        public string EmployeeName { get; set; } = "";


        [Display(Name ="Vacation Balance")]
        public int VacationBalance { get; set; }


        [Display(Name = "From Date")]
        public DateTime StartDate { get; set; }


        [Display(Name = "To Date")]
        public DateTime EndDate { get; set; }

        
        [Display(Name = "Total Vacation")]
        public int TotalVacation { get; set; }


        public int Total { get; set; }

    }
}
