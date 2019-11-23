using System;
using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class ReportsController : Controller
    {
        readonly DicServerContext _context;

        public ReportsController(DicServerContext context) => _context = context;

        // GET: Admin/Reports
        public async Task<IActionResult> Index() =>
            View(await _context.Reports.OrderBy(r => r.Manufacturer).ThenBy(r => r.Model).ThenBy(r => r.Revision).
                                ThenBy(r => r.CompactFlash).ThenBy(r => r.Type).ToListAsync());

        // GET: Admin/Reports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            UploadedReport uploadedReport = await _context.Reports.FirstOrDefaultAsync(m => m.Id == id);

            if(uploadedReport == null)
            {
                return NotFound();
            }

            return View(uploadedReport);
        }

        // GET: Admin/Reports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            UploadedReport uploadedReport = await _context.Reports.FindAsync(id);

            if(uploadedReport == null)
            {
                return NotFound();
            }

            return View(uploadedReport);
        }

        // POST: Admin/Reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, [Bind("UploadedWhen,Id,CompactFlash,Manufacturer,Model,Revision,Type")]
            UploadedReport uploadedReport)
        {
            if(id != uploadedReport.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(uploadedReport);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!UploadedReportExists(uploadedReport.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(uploadedReport);
        }

        // GET: Admin/Reports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            UploadedReport uploadedReport = await _context.Reports.FirstOrDefaultAsync(m => m.Id == id);

            if(uploadedReport == null)
            {
                return NotFound();
            }

            return View(uploadedReport);
        }

        // POST: Admin/Reports/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            UploadedReport uploadedReport = await _context.Reports.FindAsync(id);
            _context.Reports.Remove(uploadedReport);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool UploadedReportExists(int id) => _context.Reports.Any(e => e.Id == id);

        public IActionResult Find(int id, string manufacturer, string model, string bus) =>
            throw new NotImplementedException();
    }
}