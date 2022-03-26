using EFDemoIntroduction.Models;
using System;
using System.Linq;

namespace EFDemoIntroduction
{
    class Program
    {
        static void Main(string[] args)
        {
            //Database first
            
            //Making a Select 
            var db = new SoftUniContext();
            var employees = db.Employees.Where(x => x.Salary > 20000).Select(x => new { x.FirstName, x.Salary }).ToList();
            foreach (var item in employees)
            {
                Console.WriteLine($"{item.FirstName} {item.Salary}");
            }


            //Create 
            var town = new Town() { Name = "Buderich" };
            db.Towns.Add(town);
            db.SaveChanges();

            //Update
            var employee = db.Employees.Where(x => x.EmployeeId == 1).FirstOrDefault();
            employee.FirstName = "Gosho";
            db.SaveChanges();

            //Delete
            var employeeToDelete = db.Employees.Where(x => x.EmployeeId == 1).FirstOrDefault();
            db.Employees.Remove(employeeToDelete);
            db.SaveChanges();


        }
    }
}
