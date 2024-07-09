namespace CBP.Data.Models
{
    public class Company : Entity
    {
        public string Name { get; set; }
        public DateTime Established { get; set; }
        //public BusinessType BusinessType { get; set; }
        public string BusinessType { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } = [];
    }
}
