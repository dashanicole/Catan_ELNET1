using CATAN_LabActivity1.Models;
using Microsoft.AspNetCore.Mvc;

namespace CATAN_LabActivity1.Controllers
{
    public class StudentController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            //Student student = new Student()
            //{
            //    Id = 123456789,
            //    Name = "Diether D. Catan",
            //    Course = "BSCS"
            //};

            //return View(student);

            return View();
        }

        [HttpPost]
        public IActionResult Index(Student pupil)
        {
            return View(pupil);
        }
    }
}
