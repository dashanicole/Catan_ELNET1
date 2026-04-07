using System.ComponentModel.DataAnnotations;

namespace Clinic_WebAppv2.Models
{
    public class Doctor
    {
        [Key] public int docID { get; set; }
        public string docFName { get; set; } = "";
        public string docLName { get; set; } = "";
        public string docAddress { get; set; } = "";
        public string docSpecial { get; set; } = "";
    }
}
