using Clinic_WebAppv2.Data;
using Clinic_WebAppv2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Clinic_WebAppv2.Controllers
{
    public class ConsultationsController : Controller
    {
        private readonly AppDbContext _context;
        public ConsultationsController(AppDbContext context) => _context = context;

        // 1. READ & SEARCH
        public async Task<IActionResult> Index(string search)
        {
            var data = _context.Consultations
                .Include(c => c.Doctor)
                .Include(c => c.Patient)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                data = data.Where(c => c.Patient!.patLName.Contains(search) || c.Doctor!.docLName.Contains(search));
            }

            ViewBag.Count = await data.CountAsync();
            return View(await data.ToListAsync());
        }

        // 2. CREATE: Show the form
        public IActionResult Create()
        {
            ViewBag.docID = new SelectList(_context.Doctors, "docID", "docLName");
            ViewBag.patID = new SelectList(_context.Patients, "patID", "patLName");
            return View();
        }

        // 3. CREATE: Save to Database
        [HttpPost]
        public async Task<IActionResult> Create(Consultation con)
        {
            if (ModelState.IsValid)
            {
                _context.Add(con);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.docID = new SelectList(_context.Doctors, "docID", "docLName", con.docID);
            ViewBag.patID = new SelectList(_context.Patients, "patID", "patLName", con.patID);
            return View(con);
        }

        // 4. UPDATE: Show form
        public async Task<IActionResult> Edit(int id)
        {
            var con = await _context.Consultations.FindAsync(id);
            if (con == null) return NotFound();
            ViewBag.docID = new SelectList(_context.Doctors, "docID", "docLName", con.docID);
            ViewBag.patID = new SelectList(_context.Patients, "patID", "patLName", con.patID);
            return View(con);
        }

        // 5. UPDATE: Save changes
        [HttpPost]
        public async Task<IActionResult> Edit(Consultation con)
        {
            if (ModelState.IsValid)
            {
                _context.Update(con);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.docID = new SelectList(_context.Doctors, "docID", "docLName", con.docID);
            ViewBag.patID = new SelectList(_context.Patients, "patID", "patLName", con.patID);
            return View(con);
        }

        // 6. DELETE
        public async Task<IActionResult> Delete(int id)
        {
            var con = await _context.Consultations.FindAsync(id);
            if (con != null)
            {
                _context.Consultations.Remove(con);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
