using CBP.Services;
using CBP.Services.Extensions;
using CBP.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CBP.Controllers
{
    public class EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger) : Controller
    {
        private readonly IEmployeeService _employeeService = employeeService;
        private readonly ILogger<EmployeesController> _logger = logger;

        public IActionResult Index()
        {
            return View(new EmployeeViewModel());
        }

        [HttpPost]
        public JsonResult Index(EmployeeViewModel model)
        {
            try
            {
                var entity = _employeeService.Add(model);
                return Json(new { entity.Id, entity.FirstName, entity.LastName, entity.Email });
            }
            catch (HandledException ex)
            {
                return Json(new { ex.Message, ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Add Employee Post error");
                return Json(new { message = "There was an unexpected error" });
            }
        }

        [HttpGet]
        [Route("Employees/Get")]
        public JsonResult Get()
        {
            return new JsonResult(_employeeService.Get());
        }

        [HttpGet]
        [Route("Employees/Get/{email}")]
        public JsonResult Get(string email)
        {
            return new JsonResult(_employeeService.Get(email));
        }

        [HttpGet]
        [Route("Employees/Get/{firstName}/{lastName}")]
        public JsonResult Get(string firstName, string lastName)
        {
            return new JsonResult(_employeeService.Get(firstName, lastName));
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
