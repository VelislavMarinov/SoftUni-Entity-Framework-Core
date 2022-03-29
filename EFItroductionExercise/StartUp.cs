using System;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new SoftUniContext();

            
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var latestProjects = context.Projects.OrderByDescending(x => x.EndDate.Value).Take(10).ToList();
            var sb = new StringBuilder();
            foreach (var project in latestProjects)
            {
                sb.AppendLine($"{project.Name}\n{project.Description}\n{project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt")}");
            }

            return sb.ToString().TrimEnd();
        }


        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments.Include(x => x.Employees).Where(x => x.Employees.Count() > 5).OrderBy(x => x.Employees.Count())
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    x.Name,
                    managerFirstName = x.Manager.FirstName,
                    managerLastName = x.Manager.LastName,
                    employees = x.Employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList()
                }).ToList();

            var sb = new StringBuilder();

            Console.WriteLine(departments.Count());

            foreach (var department in departments)
            {
               sb.AppendLine($"{department.Name} - {department.managerFirstName} {department.managerLastName}");
               foreach (var employee in department.employees)
               {
                   sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
               }
            }

            

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {

            var employee = context.Employees.Include(x => x.EmployeesProjects).Select(x => new
            {
                x.EmployeeId,
                x.FirstName,
                x.LastName,
                x.JobTitle,
                projects = x.EmployeesProjects.OrderBy(p => p.Project.Name).Select(p => p.Project.Name)
            }).Where(x => x.EmployeeId == 147).FirstOrDefault();
            var sb = new StringBuilder();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");


            foreach (var project in employee.projects)
            {
                sb.AppendLine($"{project}"); 
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var adresses = context.Addresses.Include(x => x.Employees).OrderByDescending(x => x.Employees.Count()).ThenBy(x => x.Town.Name)
                .Select(x => new 
                 { 
                    x.AddressText,
                    count = x.Employees.Count,
                    townName = x.Town.Name,
                 })
                .Take(10).ToList();

            var sb = new StringBuilder();

            foreach (var adress in adresses)
            {
                sb.AppendLine($"{adress.AddressText}, {adress.townName} - {adress.count} employees");
            }


            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees.Take(10).Include(x => x.EmployeesProjects).ThenInclude(x => x.Project)
                .Where(x => x.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    managerFirstName = x.Manager.FirstName,
                    managerLastName = x.Manager.LastName,
                    projects = x.EmployeesProjects.Select(p => new
                    {
                        p.Project.Name,
                        p.Project.StartDate,
                        p.Project.EndDate,
                        
                    }),


                })
                .ToList();
            var sb = new StringBuilder();

            foreach (var employee in employees)
            {

                sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.managerFirstName} {employee.managerLastName}");
                foreach (var project in employee.projects)
                {
                    if (project.EndDate != null)
                    {
                        sb.AppendLine($"--{project.Name} - {project.StartDate.ToString("M/d/yyyy h:mm:ss tt")} - {project.EndDate?.ToString("M/d/yyyy h:mm:ss tt")}");
                    }
                    else
                    {
                        sb.AppendLine($"--{project.Name} - {project.StartDate.ToString("M/d/yyyy h:mm:ss tt")} - not finished");
                    }
                }
                
            }


            return sb.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var newAdress = new Address();
            newAdress.AddressText = "Vitoshka 15";
            newAdress.TownId = 4;
            context.Addresses.Add(newAdress);

            var nakov = context.Employees.Where(x => x.LastName == "Nakov").FirstOrDefault();
            nakov.Address = newAdress;
            context.SaveChanges();

            var employees = context.Employees.Select(x => new { x.AddressId, AddresText = x.Address.AddressText }).OrderByDescending(x => x.AddressId).Take(10).ToList();
            var sb = new StringBuilder();
            foreach (var adress in employees)
            {
                sb.AppendLine($"{adress.AddresText}");
            }






            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees.Select(x => new { x.FirstName, x.Salary, x.LastName ,departmentName = x.Department.Name })
                           .Where(x => x.departmentName == "Research and Development")
                           .OrderBy(x => x.Salary)
                           .ThenByDescending(x => x.FirstName);
            var sb = new StringBuilder();
            
            foreach (var employee in employees)
            {
                
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.departmentName} - ${employee.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees.Select(x => new 
            {
               x.EmployeeId,
               x.FirstName,
               x.MiddleName,
               x.LastName,
               x.JobTitle,
               x.Salary
            
            }).OrderBy(x => x.EmployeeId).ToList();
            StringBuilder text = new StringBuilder();
            foreach (var employee in employees)
            {
                text.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {(employee.Salary):F2}");
            }

            return text.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var emloyees = context.Employees.Where(x => x.Salary > 50000).Select(x => new { x.FirstName, x.Salary }).OrderBy(x => x.FirstName).ToList();
            var sb = new StringBuilder();
            foreach (var employee in emloyees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
