using Clinic_WebAppv2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_WebAppv2.Controllers
{
    public class InquiryController : Controller
    {
        private readonly AppDbContext _context;
        public InquiryController(AppDbContext db) => _context = db;

        public async Task<IActionResult> Index(string? special, int? minAge, int? maxAge, int? patID, int? docID, DateTime? fromDate, DateTime? toDate)
        {
            var consults = _context.Consultations
                .Include(c => c.Doctor)
                .Include(c => c.Patient)
                .AsQueryable();

            // 1. Doctors by specific specialization
            if (!string.IsNullOrEmpty(special))
                consults = consults.Where(c => c.Doctor!.docSpecial.Contains(special));

            // 2. Patients within a specific age range
            if (minAge.HasValue || maxAge.HasValue)
            {
                var today = DateTime.Today;
                if (minAge.HasValue)
                {
                    var maxBDate = today.AddYears(-minAge.Value);
                    consults = consults.Where(c => c.Patient!.patBDate <= maxBDate);
                }
                if (maxAge.HasValue)
                {
                    var minBDate = today.AddYears(-maxAge.Value - 1);
                    consults = consults.Where(c => c.Patient!.patBDate > minBDate);
                }
            }

            // 3. Consultations for specified patient ID
            if (patID.HasValue)
                consults = consults.Where(c => c.patID == patID.Value);

            // 4. Consultations for specified doctor ID
            if (docID.HasValue)
                consults = consults.Where(c => c.docID == docID.Value);

            // 5. Consultations within a specified date range
            if (fromDate.HasValue)
                consults = consults.Where(c => c.consultDate >= fromDate.Value);
            if (toDate.HasValue)
                consults = consults.Where(c => c.consultDate <= toDate.Value);

            // Requirement: Use Aggregate Function COUNT
            ViewBag.Count = await consults.CountAsync();
            return View(await consults.ToListAsync());
        }
    }
}
