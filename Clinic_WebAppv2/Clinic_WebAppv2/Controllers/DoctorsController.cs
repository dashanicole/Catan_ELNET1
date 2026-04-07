using Clinic_WebAppv2.Models;
using Clinic_WebAppv2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinic_WebAppv2.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly AppDbContext _context;
        public DoctorsController(AppDbContext context) => _context = context;

        // READ & SEARCH
        public async Task<IActionResult> Index(string searchName)
        {
            var doctors = from d in _context.Doctors select d;
            if (!string.IsNullOrEmpty(searchName))
            {
                doctors = doctors.Where(s => s.docLName.Contains(searchName) || s.docSpecial.Contains(searchName));
            }
            // Aggregate function COUNT for the UI requirement
            ViewBag.Count = await doctors.CountAsync();
            return View(await doctors.ToListAsync());
        }

        // CREATE (GET)
        public IActionResult Create() => View();

        // CREATE (POST)
        [HttpPost]
        public async Task<IActionResult> Create(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // UPDATE (GET)
        public async Task<IActionResult> Edit(int id) => View(await _context.Doctors.FindAsync(id));

        // UPDATE (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Update(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // DELETE
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}