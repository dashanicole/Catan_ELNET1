using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinic_WebApp.Models;

namespace Clinic_WebApp.Controllers
{
    public class ConsultationsController : Controller
    {
        private readonly AppDbContext _context;

        public ConsultationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Consultations
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Consultations.Include(c => c.Doc).Include(c => c.Pat);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Consultations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultations
                .Include(c => c.Doc)
                .Include(c => c.Pat)
                .FirstOrDefaultAsync(m => m.ConsultId == id);
            if (consultation == null)
            {
                return NotFound();
            }

            return View(consultation);
        }

        // GET: Consultations/Create
        public IActionResult Create()
        {
            ViewData["DocId"] = new SelectList(_context.Doctors, "DocId", "DocId");
            ViewData["PatId"] = new SelectList(_context.Patients, "PatId", "PatId");
            return View();
        }

        // POST: Consultations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConsultId,PatId,DocId,ConsultDate,Diagnosis,Prescription")] Consultation consultation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(consultation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DocId"] = new SelectList(_context.Doctors, "DocId", "DocId", consultation.DocId);
            ViewData["PatId"] = new SelectList(_context.Patients, "PatId", "PatId", consultation.PatId);
            return View(consultation);
        }

        // GET: Consultations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation == null)
            {
                return NotFound();
            }
            ViewData["DocId"] = new SelectList(_context.Doctors, "DocId", "DocId", consultation.DocId);
            ViewData["PatId"] = new SelectList(_context.Patients, "PatId", "PatId", consultation.PatId);
            return View(consultation);
        }

        // POST: Consultations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConsultId,PatId,DocId,ConsultDate,Diagnosis,Prescription")] Consultation consultation)
        {
            if (id != consultation.ConsultId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consultation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultationExists(consultation.ConsultId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DocId"] = new SelectList(_context.Doctors, "DocId", "DocId", consultation.DocId);
            ViewData["PatId"] = new SelectList(_context.Patients, "PatId", "PatId", consultation.PatId);
            return View(consultation);
        }

        // GET: Consultations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultations
                .Include(c => c.Doc)
                .Include(c => c.Pat)
                .FirstOrDefaultAsync(m => m.ConsultId == id);
            if (consultation == null)
            {
                return NotFound();
            }

            return View(consultation);
        }

        // POST: Consultations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation != null)
            {
                _context.Consultations.Remove(consultation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultationExists(int id)
        {
            return _context.Consultations.Any(e => e.ConsultId == id);
        }
    }
}
