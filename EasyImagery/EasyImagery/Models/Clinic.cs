using System.ComponentModel.DataAnnotations;

namespace EasyImagery.Models
{
    public class Clinic
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Display(Name = "Manager Id")]
        public string? ManagerId { get; set; }

        public Manager? Manager { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Address { get; set; }

        public ICollection<Physician>? Physicians { get; set; }
    }
}
