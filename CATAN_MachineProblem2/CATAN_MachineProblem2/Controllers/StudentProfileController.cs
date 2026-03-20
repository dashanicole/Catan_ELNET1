using Microsoft.AspNetCore.Mvc;
using CATAN_MachineProblem2.Data;
using CATAN_MachineProblem2.Models;

namespace CATAN_MachineProblem2.Controllers
{
    public class StudentProfileController : Controller
    {
        private readonly AppDbContext _context;

        public StudentProfileController(AppDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public IActionResult Index()
        {
            var students = _context.StudentProfiles.ToList();
            return View(students);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(StudentProfile student)
        {
            if (ModelState.IsValid)
            {
                _context.StudentProfiles.Add(student);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        private string GetHonor(int grade)
        {
            if (grade >= 98)
            {
                return "Summa Cum Laude";
            }
            else if (grade >= 95)
            {
                return "Magna Cum Laude";
            }
            else if (grade >= 90)
            {
                return "Cum Laude";
            }
            else
            {
                return "No Honor";
            }
        }
    }
}