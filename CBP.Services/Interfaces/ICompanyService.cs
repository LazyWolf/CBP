using CBP.Data.Models;
using CBP.Services.Models;

namespace CBP.Services
{
    public interface ICompanyService
    {
        Company Add(CompanyViewModel model);
        IQueryable<Company> Find(string name);
        IQueryable<Company> Get();
        Company? Get(long id);
        Company? Get(string name);
    }
}