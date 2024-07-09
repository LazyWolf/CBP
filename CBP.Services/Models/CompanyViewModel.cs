using CBP.Data;

namespace CBP.Services.Models
{
    public sealed class CompanyViewModel
    {
        public string Name { get; set; }
        public DateTime Established { get; set; }
        public BusinessType BusinessType { get; set; }
        public Dictionary<long, string> Employees { get; set; }
    }
}
