using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EasyImagery.Models
{
    public class Physician : IdentityUser
    {
        [Required]
        [PersonalData]
        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public int? ClinicId { get; set; }
        public Clinic? Clinic { get; set; }

        public ICollection<Timeslot>? Timeslots { get; set; }
    }
}
