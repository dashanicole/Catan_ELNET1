using System.ComponentModel.DataAnnotations;

namespace Clinic_WebAppv2.Models
{
    public class Patient
    {
        [Key] public int patID { get; set; }
        public string patFName { get; set; } = "";
        public string patLName { get; set; } = "";
        public DateTime patBDate { get; set; }
        public string patTelNo { get; set; } = "";
    }
}
