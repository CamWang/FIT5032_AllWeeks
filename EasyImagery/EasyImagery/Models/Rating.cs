﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EasyImagery.Models;

public partial class Rating
{
    public int RatingId { get; set; }

    public string PatientId { get; set; }

    public string PhysicianId { get; set; }

    public int StarRating { get; set; }
}