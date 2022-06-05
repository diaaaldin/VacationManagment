using System.ComponentModel.DataAnnotations;

namespace VacationManagment.Models
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}
