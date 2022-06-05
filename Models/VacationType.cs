using System.ComponentModel.DataAnnotations;

namespace VacationManagment.Models
{
    public class VacationType:EntityBase
    {
        [StringLength(200)]
        [Display(Name = "Vacation Name")]
        public string VacationName { get; set; } = "";
        [Display(Name = "Vacation Color")]
        [StringLength(7)]
        public string backgroundColor { get; set; } = "";
        [Display(Name = "Number Days")]
        public int NumberDays { get; set; }
    }
}
