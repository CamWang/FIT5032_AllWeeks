﻿using System.ComponentModel.DataAnnotations;

namespace EasyImagery.Models
{
    public class Timeslot
    {
        public long Id { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? StartDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? EndDate { get; set; }

        public string? PhysicianId { get; set; }
        public ApplicationUser? Physician { get; set; }

        public string? PatientId { get; set; }
        public ApplicationUser? Patient { get; set; }

        [Range(0, 10)]
        public int? Rating { get; set; }

        public byte[]? ImageData { get; set; }
    }
}
