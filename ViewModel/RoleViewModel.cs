using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace VacationManagment.ViewModel
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        
        [NotNull]
        [MinLength(1)]
        [Required]
        public string Name { get; set; }

        [NotMapped]
        [MinLength(1)]
        [NotNull]
        [Required]
        public List<string> Claims { get; set; }
    }
}
