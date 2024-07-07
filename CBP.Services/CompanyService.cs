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

        public IQueryable<Company> Get()
        {
            return _ctx.Companies;
        }

        public Company? Get(long id)
        {
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

        public Company Add(CompanyViewModel model)
        {
            Validate(model);

            var entity = _ctx.Companies.Add(new Company
            {
                Name = model.Name,
                Established = DateTime.UtcNow,
                BusinessType = model.BusinessType,
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
