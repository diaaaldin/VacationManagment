
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VacationManagment.Models;

namespace VacationManagment.Data
{
    public class VacationDbContext : IdentityDbContext
    {
        public VacationDbContext(DbContextOptions<VacationDbContext> options):base(options)
        {

        }
    
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<VacationType> VacationTypes { get; set; }
        public DbSet<RequestVacation> RequestVacations { get; set; }
        public DbSet<VacationPlan> VacationPlans { get; set; }

    }
}
