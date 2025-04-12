using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace EFCoreModelApp;

public class CompanyContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=company.db");
    }
}