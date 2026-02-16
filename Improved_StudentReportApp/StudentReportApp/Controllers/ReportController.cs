using Microsoft.AspNetCore.Mvc;
using StudentReportApp.Models;
using System.Text.Json;

namespace StudentReportApp.Controllers
{
    public class ReportController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new StudentReport());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(StudentReport report)
        {
            TempData["report"] = JsonSerializer.Serialize(report);

            return RedirectToAction("Result");
        }

        [HttpGet]
        public IActionResult Result()
        {
            if (TempData["report"] is not string json)
                return RedirectToAction("Index");

            var report = JsonSerializer.Deserialize<StudentReport>(json) ?? new StudentReport();

            TempData.Keep("report");

            return View(report);
        }
    }
}