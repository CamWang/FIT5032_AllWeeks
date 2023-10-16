using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EasyImagery.Models
{
    public class Manager : IdentityUser
    {
        [Required]
        [PersonalData]
        [MaxLength(100)]
        public string? Name { get; set; }

        public int? ClinicId { get; set; }
        public Clinic? Clinic { get; set; }
    }
}
