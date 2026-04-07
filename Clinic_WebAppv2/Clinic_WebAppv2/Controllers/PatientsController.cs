using Clinic_WebAppv2.Data;
using Clinic_WebAppv2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinic_WebAppv2.Controllers
{
    public class PatientsController : Controller
    {
        private readonly AppDbContext _context;
        public PatientsController(AppDbContext context) => _context = context;

        // READ & SEARCH
        public async Task<IActionResult> Index(string searchPat)
        {
            var patients = from p in _context.Patients select p;
            if (!string.IsNullOrEmpty(searchPat))
            {
                patients = patients.Where(s => s.patLName.Contains(searchPat));
            }
            ViewBag.Count = await patients.CountAsync();
            return View(await patients.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        public async Task<IActionResult> Edit(int id) => View(await _context.Patients.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Update(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var p = await _context.Patients.FindAsync(id);
            if (p != null)
            {
                _context.Patients.Remove(p);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}