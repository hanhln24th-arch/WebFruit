using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Database.Models
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        [Required]
        [MaxLength(150)]
        public string? Code { get; set; }
        [ForeignKey("CategoryId")]
        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<Autherized> Autherizeds { get; set; } = new HashSet<Autherized>();
    }
}
