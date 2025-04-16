using CBP.Data;
using CBP.Data.Models;
using CBP.Services.Extensions;
using CBP.Services.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CBP.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IDataContext _ctx;
        private readonly ILogger<CompanyService> _logger;

        public CompanyService(IDataContext dataContext, ILogger<CompanyService> logger)
        {
            _ctx = dataContext;
            _logger = logger;
        }

        public List<Company> Get()
        {
            return _ctx.Companies.ToList();
        }

        public Company Get(long id)
        {
            // TODO: Don't ignore the green squigglies. Null safety is important
            return _ctx.Companies.Find(id);
        }

        public Company? Get(string name)
        {
            return _ctx.Companies.FirstOrDefault(c => EF.Functions.Like(c.Name, name));
        }

        public IQueryable<Company> Find(string name)
        {
            return _ctx.Companies.Where(c => EF.Functions.Like(c.Name, $"%{name}%"));
        }

        /// <summary>
        /// Add a corporation
        /// </summary>
        /// <param name="model">Model to create new company from</param>
        /// <param name="model">Business type of company</param>
        /// <returns></returns>
        public Company AddCorporation(CompanyViewModel model)
        {
            Validate(model);

            var entity = _ctx.Companies.Add(new Company
            {
                Name = model.Name,
                Established = DateTime.UtcNow,
                BusinessType = "Corporation", // TODO: Avoid magic strings
            }).Entity;

            _ctx.SaveChanges();

            return entity;
        }

        public Company AddLimitedLiabilityCompany(CompanyViewModel model)
        {
            // TODO: Avoid code duplication when possible
            // https://bitbucket.org/saberinsystems/sar-msrb-v3.1/commits/7df5b094dcf0fd255fcc3c5d7cd7cf91a03b9030#Lmsrb.services/Services/TradeService.csT160
            Validate(model);

            var entity = _ctx.Companies.Add(new Company
            {
                Name = model.Name,
                Established = DateTime.UtcNow,
                BusinessType = "LimitedLiabilityCompany",
            }).Entity;

            _ctx.SaveChanges();

            return entity;
        }

        public Company AddNonProfitCompany(CompanyViewModel model)
        {
            Validate(model);

            var entity = _ctx.Companies.Add(new Company
            {
                Name = model.Name,
                Established = DateTime.UtcNow,
                BusinessType = "NonProfit",
            }).Entity;

            _ctx.SaveChanges();

            return entity;
        }

        public CompanyViewModel? UpdateCompanyEmployees(CompanyViewModel companyModel)
        {
            // TODO: Avoid over-nesting with defensive coding
            if (companyModel != null)
            {
                if (companyModel.Employees.Any())
                {
                    foreach(var employeeKey in companyModel.Employees.Select(e => e.Key))
                    {
                        // Do something with employees?
                        var empoloyee = _ctx.Employees.Find(employeeKey);
                        if (empoloyee == null)
                        {
                            _logger.LogError("Employee not found with Key {Key}", employeeKey);
                            continue;
                        }
                        _ctx.Employees.Update(empoloyee);
                    }

                    _ctx.SaveChanges();
                }
            }

            return companyModel;
        }

        private void Validate(CompanyViewModel model)
        {
            var errors = new List<string>();

            // Name
            // TODO: Using IsNullOrWhiteSpace vs IsNullOrEmpty where relevant
            if (String.IsNullOrWhiteSpace(model.Name))
            {
                errors.Add("Name must not be empty");
            }
            else if (Get(model.Name) is not null)
            {
                errors.Add("A company with that name already exists");
            }

            // Throw if errors
            if (errors.Any())
            {
                throw new HandledException("One or more errors occurred", errors);
            }
        }

        public void SqlCommand(string userInput)
        {
            // EG1
            using (SqlConnection cn1 = new SqlConnection("Data Source=svr01;Initial Catalog=XYZ;Persist Security Info=True;User ID=sa;Password=SomePassword"))
            {
                cn1.Open();
                using (SqlCommand cm1 = new SqlCommand())
                {
                    cm1.Connection = cn1;
                    cm1.CommandText = "INSERT INTO accountingfundfield([fund]) VALUES ('" + userInput + "')";
                    // TODO: Avoid SQL Injection vulnerabilities
                    // cm1.CommandText = "INSERT INTO accountingfundfield([fund]) VALUES (@fund)";
                    // cm1.Parameters.AddWithValue("@fund", userInput);
                    cm1.ExecuteNonQuery();
                }
            }

            // EG2
            var sql = "INSERT INTO accountingfundfield([fund]) VALUES ('" + userInput + "')";
            _ctx.Database.ExecuteSqlRaw(sql);

            // sql0 = "INSERT INTO accountingfundfield([fund]) VALUES (@p0)";
            // _ctx.Database.ExecuteSqlRaw(sql, userInput);
        }
    }
}
