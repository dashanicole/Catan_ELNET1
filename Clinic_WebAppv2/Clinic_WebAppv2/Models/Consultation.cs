using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_WebAppv2.Models
{
    public class Consultation
    {
        [Key] public int consultID { get; set; }
        public int patID { get; set; }
        [ForeignKey("patID")] public Patient? Patient { get; set; }
        public int docID { get; set; }
        [ForeignKey("docID")] public Doctor? Doctor { get; set; }
        public DateTime consultDate { get; set; }
        public string diagnosis { get; set; } = "";
        public string prescription { get; set; } = "";
    }
}
