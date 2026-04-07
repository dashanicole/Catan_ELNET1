using System;
using System.Collections.Generic;

namespace Clinic_WebApp.Models;

public partial class Consultation
{
    public int ConsultId { get; set; }

    public int? PatId { get; set; }

    public int? DocId { get; set; }

    public DateTime? ConsultDate { get; set; }

    public string? Diagnosis { get; set; }

    public string? Prescription { get; set; }

    public virtual Doctor? Doc { get; set; }

    public virtual Patient? Pat { get; set; }
}
