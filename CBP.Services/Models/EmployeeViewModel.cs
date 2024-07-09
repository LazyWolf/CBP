using CBP.Data.Models;

namespace CBP.Services.Models
{
    public sealed class EmployeeViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public long CompanyId { get; set; }

        public IQueryable<Company> Companies { get; set; } = [];
    }
}
