using System;
using System.Collections.Generic;

namespace Clinic_WebApp.Models;

public partial class Doctor
{
    public int DocId { get; set; }

    public string? DocFname { get; set; }

    public string? DocLname { get; set; }

    public string? DocAddress { get; set; }

    public string? DocSpecialization { get; set; }

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();
}
