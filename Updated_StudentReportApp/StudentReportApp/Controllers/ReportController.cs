using Microsoft.AspNetCore.Mvc;
using StudentReportApp.Models;

namespace StudentReportApp.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            StudentReport report = new StudentReport()
            {
                StudentId = 123456789,
                StudentName = "Dasha Nicole",
                SubjectNames = new List<string> { "English", "Filipino", "Math", "Science", "MAPEH"},

                // Passed
                StudentGrades = new List<double> { 1.2, 1.0, 1.4, 1.3, 1.1 }

                // Failed
                //StudentGrades = new List<double> { 3.0, 2.8, 2.9, 3.5, 3.9 }
            };

            return View(report);
        }
    }
}