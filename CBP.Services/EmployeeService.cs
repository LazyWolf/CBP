using CBP.Data;
using CBP.Data.Models;
using CBP.Services.Extensions;
using CBP.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CBP.Services
{
    public partial class EmployeeService : IEmployeeService
    {
        private readonly IDataContext _ctx;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IDataContext dataContext, ILogger<EmployeeService> logger)
        {
            _ctx = dataContext;
            _logger = logger;
        }

        public IQueryable<Employee> Get()
        {
            return _ctx.Employees;
        }

        public Employee? Get(long id)
        {
            return _ctx.Employees.Find(id);
        }

        public Employee? Get(string firstName, string lastName)
        {
            return _ctx.Employees.FirstOrDefault(e =>
                   EF.Functions.Like(e.FirstName, firstName)
                && EF.Functions.Like(e.LastName, lastName)
            );
        }

        public Employee? Get(string email)
        {
            return _ctx.Employees.FirstOrDefault(e =>
                   EF.Functions.Like(e.Email, email)
            );
        }

        public IQueryable<Employee> Find(string searchText)
        {
            return _ctx.Employees.Where(e =>
                   EF.Functions.Like(e.Email, $"%{searchText}%")
                || EF.Functions.Like(e.FirstName, $"%{searchText}%")
                || EF.Functions.Like(e.LastName, $"%{searchText}%")
            );
        }

        public IQueryable<Employee> FindName(string searchText)
        {
            return _ctx.Employees.Where(e =>
                   EF.Functions.Like(e.FirstName, $"%{searchText}%")
                || EF.Functions.Like(e.LastName, $"%{searchText}%")
            );
        }

        public IQueryable<Employee> FindEmail(string searchText)
        {
            return _ctx.Employees.Where(e =>
                   EF.Functions.Like(e.Email, $"%{searchText}%")
            );
        }

        public Employee Add(EmployeeViewModel model)
        {
            Validate(model);

            var entity = _ctx.Employees.Add(new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                HireDate = DateTime.UtcNow,
            }).Entity;

            _ctx.SaveChanges();

            return entity;
        }

        private void Validate(EmployeeViewModel model)
        {
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
            else if (!EmailRegex().IsMatch(model.Email))
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
        }

        [GeneratedRegex(@"^((?!\.)[\w\-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$")]
        private static partial Regex EmailRegex();
    }
}
