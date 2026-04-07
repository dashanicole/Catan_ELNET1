using System;
using System.Collections.Generic;

namespace Clinic_WebApp.Models;

public partial class Patient
{
    public int PatId { get; set; }

    public string? PatFname { get; set; }

    public string? PatLname { get; set; }

    public DateOnly? PatBirthDate { get; set; }

    public string? PatTelNo { get; set; }

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();
}
