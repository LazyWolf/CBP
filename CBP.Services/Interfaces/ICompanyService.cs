using CBP.Data.Models;
using CBP.Services.Models;

namespace CBP.Services
{
    public interface ICompanyService
    {
        Company AddCorporation(CompanyViewModel model);
        Company AddLimitedLiabilityCompany(CompanyViewModel model);
        Company AddNonProfitCompany(CompanyViewModel model);
        IQueryable<Company> Find(string name);
        List<Company> Get();
        Company? Get(long id);
        Company? Get(string name);
    }
}