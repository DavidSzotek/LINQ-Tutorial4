/*
* https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/standard-query-operators-overview
* 
* ***** SORTING OPERATORS *****
* 
* - OrderBy, OrderByDescending
* 
* - ThenBy, ThenByDescending
* 
* 
* 
* ***** GROUPING OPERATORS *****
* 
* - GroupBy (deffered)
* 
* - ToLookup (immediate)
* 
* 
* 
* ***** QUANTIFIER OPERATORS *****
* 
* - All = returns true if all records meet the conditions
* 
* - Any = returns true if at least one record meets the conditon
* 
* - Contains = returns true if collection contains searched item
*            = Comparer class implementing IEqualityComparer<ComparedObject> has to be implemented in order to let the compiler know 
*              how to uniquely identify object (GetHashCode(obj x))
*              and how to determine wether two objects are equal (Equals(obj x, obj y))
*            = Instance of Comparer Class has to be then passed to Contains operator as argument
* 
* 
* 
* ***** FILTER OPERATORS *****
* 
* - OfType = is useful when I have Heterogeneous Data Collections (Collection containing different data types)
*          = filters records based on their data type
*          
* - Where
* 
* 
* 
* ***** ELEMENT OPERATORS *****
* 
* - ElementAt
* 
* - ElementAtOrDefault
* 
* - First
* 
* - FirstOrDefault
* 
* - Last
* 
* - LastOrDefault
* 
* - Single
* 
* - SingleOrDefault
* 
*/

using System.Diagnostics.CodeAnalysis;

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
                $"Id: {result.Id,-5}" +
                $"First Name: {result.FirstName,-10} " +
                $"Last Name: {result.LastName,-10} " +
                $"Annual Salary: {result.AnnualSalary,-10}" +
                $"\tDepartment Id: {result.DepartmentId,-10}" +
                $"Department Name: {result.DepartmentName,-10}"
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

        /***** Quantifier operators - All and Any return boolean value *****/
        /* All() - If all records satisfy the condition, All() returns true */
        var annualSalaryCompare = 200000;

        bool isTrueAll = employeeList.All(e => e.AnnualSalary > annualSalaryCompare);
        if (isTrueAll)
        {
            Console.WriteLine($"All employees have annual salary higher than {annualSalaryCompare}");
        }
        else
        {
            Console.WriteLine($"Not all employees have annual salary higher than {annualSalaryCompare}");
        }

        /* Any() - If at least one record satisfies the condition, Any() returns true */
        bool isTrueAny = employeeList.Any(e => e.AnnualSalary > annualSalaryCompare);
        if (isTrueAny)
        {
            Console.WriteLine($"At least one employee has higher salary than {annualSalaryCompare}");
        }
        else
        {
            Console.WriteLine($"No employee has higher salary than {annualSalaryCompare}");
        }

        /* Contains() - returns true if any of the searched record matches any of the objects within the searched list */
        /* I need to tell the compiler how to compare user defined objects, in this case employee */

        var searchedEmployee = new Employee()
        {
            Id = 3,
            FirstName = "Douglas",
            LastName = "Roberts",
            AnnualSalary = 40000.2m,
            IsManager = false,
            DepartmentId = 1,
        };

        bool containsEmployee = employeeList.Contains(searchedEmployee, new EmployeeComparer());

        if (containsEmployee)
        {
            Console.WriteLine($"Employee record {searchedEmployee.FirstName} {searchedEmployee.LastName} was found!");
        }
        else
        {
            Console.WriteLine($"Employee record {searchedEmployee.FirstName} {searchedEmployee.LastName} was not found!");
        }

        Console.WriteLine("\n");

        /***** Filter operators - Method syntax *****/
        Console.WriteLine("***** Filter operators - Method syntax *****");

        var methodFilteredResults = employeeList.OrderByDescending(e => e.AnnualSalary).Where(e => e.AnnualSalary < 50000);

        foreach (var result in methodFilteredResults)
        {
            Console.WriteLine(
                $"Id: {result.Id,-5}" +
                $"First Name: {result.FirstName,-10} " +
                $"Last Name: {result.LastName,-10} " +
                $"Annual Salary: {result.AnnualSalary,-10}"
            );
        }

        Console.WriteLine("\n");

        /***** Filter operators - Query syntax *****/
        Console.WriteLine("***** Filter operators - Query syntax *****");

        var queryFilteredResults = from emp in employeeList
                                   join dept in departmentList
                                   on emp.DepartmentId equals dept.Id
                                   where emp.AnnualSalary > 50000
                                   orderby emp.LastName descending
                                   select new
                                   {
                                       Id = emp.Id,
                                       FullName = emp.FirstName + " " + emp.LastName,
                                       AnnualSalary = emp.AnnualSalary,
                                       DepartmentName = dept.ShortName
                                   };
     
        foreach (var result in queryFilteredResults)
        {
            Console.WriteLine(
                $"Id: {result.Id,-5}" +
                $"Name: {result.FullName,-20} " +
                $"Annual Salary: {result.AnnualSalary,-10}" +
                $"Department: {result.DepartmentName}"
            );
        }

        Console.WriteLine("\n");


        /***** Element operators *****/
        Console.WriteLine("***** Element operators *****");

        int position1 = 0;
        int position2 = 60;

        /* ElementAt() - returns record in collection at specified position - if the record doesnt exist, throws exception */
        var employeeElementAt = employeeList.ElementAt(position1);
        Console.WriteLine($"Employee at position {position1} has Id: {employeeElementAt.Id} is called {employeeElementAt.FirstName + " " + employeeElementAt.LastName}");

        /* ElementAtOrDefault() - returns record in collection at specified position - 
         * if the record doesnt exist, returns default value (null for objects and string, 0 for int etc.) */
        var employeeElementAtOrDefault = employeeList.ElementAtOrDefault(position2);
        if (employeeElementAtOrDefault != null)
        {
            Console.WriteLine($"Employee at position {position2} has Id: {employeeElementAtOrDefault.Id} is called {employeeElementAtOrDefault.FirstName + " " + employeeElementAtOrDefault.LastName}");
        }
        else
        {
            Console.WriteLine($"Employee at position {position2} doesn't exist!");
        }

        /* First, FirstOrDefault, Last, LastOrDefault - returns first or last element of a collection 
                                                      - can specify condition, in which case return first or last item that meets the condition */
        var employeeFirstOrDefault = employeeList.FirstOrDefault(a => !a.IsManager && a.AnnualSalary>=90000);
        if (employeeFirstOrDefault != null)
        {
            Console.WriteLine($"First record found is {employeeFirstOrDefault.FirstName + " " + employeeFirstOrDefault.LastName}");
        }
        else
        {
            Console.WriteLine("No record was found.");
        }
        Console.WriteLine("\n");

        /* Single, SingleOrDefault - returns element if it's the only one satysfying the condition 
                                   - otherwise returns null or throws exception */

        try {
            var employeeSingleOrDefault = employeeList.SingleOrDefault(a => !a.IsManager && a.AnnualSalary >= 70000);
            if (employeeSingleOrDefault != null)
            {
                Console.WriteLine($"Single record satisfying the condition is {employeeFirstOrDefault.FirstName + " " + employeeFirstOrDefault.LastName}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("None or more than one records satisfy the condition");
        }
        
        Console.WriteLine("\n");
    }
}

public class EmployeeComparer : IEqualityComparer<Employee>
{
    /* Equals() defines how to determine whether the 2 objects are equal */
    public bool Equals(Employee? x, Employee? y)
    {
        if(x.Id == y.Id)
        {
            return true;
        }
        return false;
    }

    /* GetHashCode() is used for uniquely identifiyng an object */
    public int GetHashCode([DisallowNull] Employee obj)
    {
        return obj.Id.GetHashCode();
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
            DepartmentId = 1,
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