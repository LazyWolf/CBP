using CBP.Services;
using CBP.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CBP.Controllers
{
    public class HomeController(IEmployeeService employeeService, ILogger<HomeController> logger) : Controller
    {
        private readonly IEmployeeService _employeeService = employeeService;
        private readonly ILogger<HomeController> _logger = logger;

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
