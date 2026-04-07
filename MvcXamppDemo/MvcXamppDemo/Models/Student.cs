using System.ComponentModel.DataAnnotations;

namespace MvcXamppDemo.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; } = "";
    }
}