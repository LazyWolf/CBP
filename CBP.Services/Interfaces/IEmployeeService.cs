using CBP.Data.Models;
using CBP.Services.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CBP.Services
{
    public interface IEmployeeService
    {
        IQueryable<Employee> Get();
        Employee? Get(long id);
        Employee? Get(string firstName, string lastName);
        Employee? Get(string email);
        IQueryable<Employee> Find(string searchText);
        IQueryable<Employee> FindEmail(string searchText);
        IQueryable<Employee> FindName(string searchText);
        Employee Add(EmployeeViewModel model);
    }
}