using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EasyImagery.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public int? ManagerClinicId { get; set; }
        public Clinic? ManagerClinic { get; set; }

        public int? PhysicianClinicId { get; set; }
        public Clinic? PhysicianClinics { get; set; }

        public ICollection<Timeslot>? PhysicianTimeslots { get; set; }

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

        public long? PatientTimeslotId { get; set; }
        public Timeslot? PatientTimeslot { get; set; }

        public string? UserType { get; set; }
    }

}
