using CBP.Services;
using CBP.Services.Extensions;
using CBP.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CBP.Controllers
{
    public class EmployeesController(IEmployeeService employeeService, ICompanyService companyService, ILogger<EmployeesController> logger) : Controller
    {
        private readonly IEmployeeService _employeeService = employeeService;
        private readonly ICompanyService _companyService = companyService;
        private readonly ILogger<EmployeesController> _logger = logger;

        public IActionResult Index()
        {
            return View(new EmployeeViewModel
            {
                Companies = _companyService.Get(),
            });
        }

        [HttpPost]
        public JsonResult Index(EmployeeViewModel model)
        {
            try
            {
                // TODO: Leave heavy logic to services
                var errors = new List<string>();

                // First Name
                if (String.IsNullOrWhiteSpace(model.FirstName))
                {
                    errors.Add("First name must not be empty");
                }

                // Last Name
                if (String.IsNullOrWhiteSpace(model.LastName))
                {
                    errors.Add("Last name must not be empty");
                }

                // Email
                if (String.IsNullOrWhiteSpace(model.Email))
                {
                    errors.Add("Email name must not be empty");
                }
                else if (!new Regex(@"^((?!\.)[\w\-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$").IsMatch(model.Email))
                {
                    errors.Add("Please type a valid email address");
                }
                else if (Get(model.Email) is not null)
                {
                    errors.Add("A user with that email already exists");
                }

                // Throw if errors
                if (errors.Any())
                {
                    throw new HandledException("One or more errors occurred", errors);
                }

                var entity = _employeeService.Add(model);
                return Json(new { entity.Id, entity.FirstName, entity.LastName, entity.Email });
            }
            catch (Exception ex)
            {
                // TODO: Don't return raw exceptions to the front-end
                _logger.LogInformation(ex, "Add Employee Post error");
                return Json(new { message = ex.Message });
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
