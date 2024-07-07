using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBP.Data.Models
{
    public class Entity
    {
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedUtc { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedUtc { get; set; }

        public virtual void PrepareSave(EntityState state)
        {
            var identityName = Thread.CurrentPrincipal?.Identity?.Name;
            var now = DateTime.UtcNow;

            if (state == EntityState.Added)
            {
                CreatedBy = identityName ?? "unknown";
                CreatedUtc = now;
            }
            else
            {
                UpdatedBy = identityName ?? "unknown";
                UpdatedUtc = now;
            }
        }
    }
}
