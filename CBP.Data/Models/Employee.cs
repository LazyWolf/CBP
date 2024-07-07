namespace CBP.Data.Models
{
    public class Employee : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime HireDate { get; set; }

        public virtual Company? Company { get; set; }
    }
}
