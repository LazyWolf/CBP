using CBP.Services;
using CBP.Services.Extensions;
using CBP.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CBP.Controllers
{
    public class CompaniesController(ICompanyService CompanyService, ILogger<CompaniesController> logger) : Controller
    {
        private readonly ICompanyService _companyService = CompanyService;
        private readonly ILogger<CompaniesController> _logger = logger;

        public IActionResult Index()
        {
            return View(new CompanyViewModel());
        }

        [HttpPost]
        public JsonResult Index(CompanyViewModel model)
        {
            try
            {
                var entity = _companyService.Add(model);
                return Json(new { entity.Id, entity.Name, entity.BusinessType });
            }
            catch (HandledException ex)
            {
                return Json(new { ex.Message, ex.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Add Company Post error");
                return Json(new { message = "There was an unexpected error" });
            }
        }

        [HttpGet]
        [Route("Companies/Get")]
        public JsonResult Get()
        {
            return new JsonResult(_companyService.Get());
        }

        [HttpGet]
        [Route("Companies/Get/{name}")]
        public JsonResult Get(string name)
        {
            return new JsonResult(_companyService.Get(name));
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
