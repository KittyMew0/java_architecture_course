using Microsoft.AspNetCore.Mvc;
using SpaWebApplication.Models;

namespace SpaWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Home()
        {
            List<Employee> employees = new List<Employee>();
            employees.Add(new Employee
            {
                Name = "A",
                Age = 30,
                Id = 1001
            });
            employees.Add(new Employee
            {
                Name = "B",
                Age = 40,
                Id = 1002
            });
            employees.Add(new Employee
            {
                Name = "C",
                Age = 50,
                Id = 1003
            });

            return View(employees);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
