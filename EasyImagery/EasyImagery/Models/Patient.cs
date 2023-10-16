using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EasyImagery.Models
{
    public class Patient : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        [PersonalData]
        public string? Name { get; set; }

        [MaxLength(200)]
        [PersonalData]
        public string? Address { get; set; }

        [MaxLength(100)]
        [PersonalData]
        public string? City { get; set; }

        [MaxLength(50)]
        [PersonalData]
        public string? State { get; set; }

        [MaxLength(20)]
        [PersonalData]
        public string? Zip { get; set; }

        [DataType(DataType.DateTime)]
        [PersonalData]
        public DateTime? Birthday { get; set; }

        public long? TimeslotId { get; set; }
        public Timeslot? Timeslot { get; set; }
    }
}
