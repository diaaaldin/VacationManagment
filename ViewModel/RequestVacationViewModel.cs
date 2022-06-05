using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VacationManagment.Models;

namespace VacationManagment.ViewModel
{
    public class RequestVacationViewModel
    {
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }
        

        [Display(Name = "VacationType")]
        public int VacationTypeId { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public string? Comment { get; set; }

        public bool Approved { get; set; }

    }
}
