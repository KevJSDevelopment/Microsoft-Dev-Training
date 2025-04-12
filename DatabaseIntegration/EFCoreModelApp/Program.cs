using EFCoreModelApp;

var context = new CompanyContext();

if (context.Departments.Count() == 0)
{
    context.Departments.Add(
        new Department
        {
            Name = "IT",
            Employees = new List<Employee>
            {
                new Employee()
                {
                    FirstName = "John",
                    LastName = "Doe",
                    HireDate = DateTime.Now,
                },
                new Employee()
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    HireDate = DateTime.Now,
                }
            }
        }
    );
    context.SaveChanges();
}


var employees = context.Employees.Where(e => e.Department.Name == "IT");
Console.WriteLine($"Employees in engineering: {employees.Count()}");