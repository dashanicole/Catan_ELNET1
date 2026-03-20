using System.ComponentModel.DataAnnotations;

namespace CATAN_MachineProblem2.Models
{
    public class StudentProfile
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Course { get; set; }

        [Range(75, 99, ErrorMessage = "Grade must be from 75 to 99.")]
        public int Grade { get; set; }
    }
}