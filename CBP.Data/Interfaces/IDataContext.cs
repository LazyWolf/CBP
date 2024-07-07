using CBP.Data.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CBP.Data
{
    public interface IDataContext
    {
        DbSet<Company> Companies { get; set; }
        DbSet<Employee> Employees { get; set; }

        DatabaseFacade Database { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
