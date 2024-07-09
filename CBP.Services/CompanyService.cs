using CBP.Data;
using CBP.Data.Models;
using CBP.Services.Extensions;
using CBP.Services.Models;
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

        // TODO: Decorating methods is helpful
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


        private void Validate(CompanyViewModel model)
        {
            var errors = new List<string>();

            // Name
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
    }
}
