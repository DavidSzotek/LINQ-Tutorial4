/*
* https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/standard-query-operators-overview
* 
* *** SORTING OPERATORS ***
* - OrderBy, OrderByDescending
* - ThenBy, ThenByDescending
* 
* *** GROUPING OPERATORS ***
* - GroupBy (deffered)
* - ToLookup (immediate)
* 
* *** QUANTIFIER OPERATORS ***
* - All
* - Any
* - Contains
* 
* *** FILTER OPERATORS ***
* - OfType
* - Where
* 
* *** ELEMENT OPERATORS ***
* - ElementAt
* - ElementAtOrDefault
* - First
* - FirstOrDefault
* - Last
* - LastOrDefault
* - Single
* - SingleOrDefault
*/

internal class Program
{
    private static void Main(string[] args)
    {
        List<Employee> employeeList = Data.GetEmployees();
        List<Department> departmentList = Data.GetDepartments();

        /***** Sorting operators - Method syntax *****/
        var methodSortingResults = employeeList.Join(departmentList, emp => emp.DepartmentId, dept => dept.Id,
            (emp, dept) => new
            {
                Id = emp.Id,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                AnnualSalary = emp.AnnualSalary,
                DepartmentId = emp.DepartmentId,
                DepartmentName = dept.LongName
            }).OrderBy(o => o.DepartmentId).ThenByDescending(o => o.AnnualSalary);

        Console.WriteLine("***** Sorting operators - Method syntax *****");

        foreach (var result in methodSortingResults)
        {
            Console.WriteLine(
                $"Id: {result.Id, -5}" +
                $"First Name: {result.FirstName,-10} " +
                $"Last Name: {result.LastName,-10} " +
                $"Annual Salary: {result.AnnualSalary,-10}" +
                $"\tDepartment Id: {result.DepartmentId, -10}" +
                $"Department Name: {result.DepartmentName, -10}"
                );
        }
        Console.WriteLine("\n");




        /***** Sorting operators - Query syntax *****/
        var querySortingResults = from emp in employeeList
                                  join dept in departmentList
                                  on emp.DepartmentId equals dept.Id
                                  orderby emp.DepartmentId descending, emp.AnnualSalary
                                  select new
                                  {
                                      Id = emp.Id,
                                      FirstName = emp.FirstName,
                                      LastName = emp.LastName,
                                      AnnualSalary = emp.AnnualSalary,
                                      DepartmentId = emp.DepartmentId,
                                      DepartmentName = dept.LongName
                                  };

        Console.WriteLine("***** Sorting operators - Query syntax *****");

        foreach (var result in querySortingResults)
        {
            Console.WriteLine(
                $"Id: {result.Id,-5}" +
                $"First Name: {result.FirstName,-10} " +
                $"Last Name: {result.LastName,-10} " +
                $"Annual Salary: {result.AnnualSalary,-10}" +
                $"\tDepartment Id: {result.DepartmentId,-10}" +
                $"Department Name: {result.DepartmentName,-10}"
                );
        }

        Console.WriteLine("\n");
        /***** Grouping operators - Query syntax *****/
        /*  GroupBy is deffered */
        /*  ToLookup is immediate */
        var queryGroupResult = from emp in employeeList
                               orderby emp.DepartmentId descending
                               group emp by emp.DepartmentId;

        Console.WriteLine("***** Grouping operator GroupBy - Query syntax *****");

        foreach (var resultGroup in queryGroupResult)
        {
            Console.WriteLine($"Department Id: {resultGroup.Key}");
            foreach (var result in resultGroup)
            {
                Console.WriteLine($"\t Employee Fullname: {result.FirstName + " " + result.LastName}");
            }
        }

        Console.WriteLine("\n");



        /***** Grouping operators - Query syntax *****/
        /*  GroupBy is deffered */
        /*  ToLookup is immediate */
        var methodGroupResult = employeeList.OrderBy(e => e.DepartmentId).ToLookup(e => e.DepartmentId).ToList();
        Console.WriteLine("***** Grouping operator ToLookup - Method syntax *****");

        foreach (var resultGroup in methodGroupResult)
        {
            Console.WriteLine($"Department Id: {resultGroup.Key}");
            foreach (var result in resultGroup)
            {
                Console.WriteLine($"\t Employee Fullname: {result.FirstName + " " + result.LastName}");
            }
        }

        Console.WriteLine("\n");

        /***** Quantifier operators *****/





        /***** Filter operators *****/





        /***** Element operators *****/
    }
}


public class Department
{
    public int Id { get; set; }
    public string ShortName { get; set; }
    public string LongName { get; set; }
}

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal AnnualSalary { get; set; }
    public bool IsManager { get; set; }
    public int DepartmentId { get; set; }
}

public static class Data
{
    public static List<Employee> GetEmployees()
    {
        List<Employee> employees = new List<Employee>();

        Employee employee = new Employee()
        {
            Id = 1,
            FirstName = "Bob",
            LastName = "Jones",
            AnnualSalary = 60000.3m,
            IsManager = true,
            DepartmentId = 1,
        };
        employees.Add(employee);
        employee = new Employee()
        {
            Id = 2,
            FirstName = "Sarah",
            LastName = "Jameson",
            AnnualSalary = 80000.1m,
            IsManager = true,
            DepartmentId = 2,
        };
        employees.Add(employee);
        employee = new Employee()
        {
            Id = 3,
            FirstName = "Douglas",
            LastName = "Roberts",
            AnnualSalary = 40000.2m,
            IsManager = false,
            DepartmentId = 2,
        };
        employees.Add(employee);
        employee = new Employee()
        {
            Id = 4,
            FirstName = "Jane",
            LastName = "Stevens",
            AnnualSalary = 30000.2m,
            IsManager = false,
            DepartmentId = 1,
        };
        employees.Add(employee);
        employee = new Employee()
        {
            Id = 5,
            FirstName = "David",
            LastName = "Szotek",
            AnnualSalary = 75000.3m,
            IsManager = true,
            DepartmentId = 3,
        };
        employees.Add(employee);
        employee = new Employee()
        {
            Id = 6,
            FirstName = "Dominik",
            LastName = "Foniok",
            AnnualSalary = 60000.1m,
            IsManager = false,
            DepartmentId = 1,
        };
        employees.Add(employee);
        employee = new Employee()
        {
            Id = 7,
            FirstName = "Klara",
            LastName = "Mezes",
            AnnualSalary = 80000.3m,
            IsManager = true,
            DepartmentId = 3,
        };
        employees.Add(employee);
        employee = new Employee()
        {
            Id = 8,
            FirstName = "Rostislav",
            LastName = "Mezes",
            AnnualSalary = 35000.3m,
            IsManager = false,
            DepartmentId = 4,
        };
        employees.Add(employee);
        employee = new Employee()
        {
            Id = 9,
            FirstName = "Juliana",
            LastName = "Szotkova",
            AnnualSalary = 90000.3m,
            IsManager = false,
            DepartmentId = 1,
        };
        employees.Add(employee);
        employee = new Employee()
        {
            Id = 10,
            FirstName = "Martin",
            LastName = "Cerny",
            AnnualSalary = 28000.3m,
            IsManager = true,
            DepartmentId = 1,
        };
        employees.Add(employee);
        employee = new Employee()
        {
            Id = 11,
            FirstName = "Lucie",
            LastName = "Zajac",
            AnnualSalary = 88000.3m,
            IsManager = true,
            DepartmentId = 3,
        };
        employees.Add(employee);
        employee = new Employee()
        {
            Id = 12,
            FirstName = "Marek",
            LastName = "Zelina",
            AnnualSalary = 21000.3m,
            IsManager = false,
            DepartmentId = 2,
        };
        employees.Add(employee);
        return employees;
    }

    public static List<Department> GetDepartments()
    {
        List<Department> departments = new List<Department>();

        Department department = new Department()
        {
            Id = 1,
            ShortName = "HR",
            LongName = "Human Resources"
        };
        departments.Add(department);

        department = new Department()
        {
            Id = 2,
            ShortName = "FN",
            LongName = "Finance"
        };
        departments.Add(department);

        department = new Department()
        {
            Id = 3,
            ShortName = "TE",
            LongName = "Technology"
        };
        departments.Add(department);

        department = new Department()
        {
            Id = 4,
            ShortName = "SC",
            LongName = "Security"
        };
        departments.Add(department);

        return departments;
    }
}