using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsbProductsController : Controller
    {
        readonly DicServerContext _context;

        public UsbProductsController(DicServerContext context) => _context = context;

        // GET: Admin/UsbProducts
        public async Task<IActionResult> Index()
        {
            IIncludableQueryable<UsbProduct, UsbVendor> dicServerContext = _context.UsbProducts.Include(u => u.Vendor);

            return View(await dicServerContext.ToListAsync());
        }

        // GET: Admin/UsbProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            UsbProduct usbProduct =
                await _context.UsbProducts.Include(u => u.Vendor).FirstOrDefaultAsync(m => m.Id == id);

            if(usbProduct == null)
            {
                return NotFound();
            }

            return View(usbProduct);
        }

        // GET: Admin/UsbProducts/Create
        public IActionResult Create()
        {
            ViewData["VendorId"] = new SelectList(_context.UsbVendors, "Id", "Id");

            return View();
        }

        // POST: Admin/UsbProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,Product,AddedWhen,ModifiedWhen,VendorId")]
                                                UsbProduct usbProduct)
        {
            if(ModelState.IsValid)
            {
                _context.Add(usbProduct);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["VendorId"] = new SelectList(_context.UsbVendors, "Id", "Id", usbProduct.VendorId);

            return View(usbProduct);
        }

        // GET: Admin/UsbProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            UsbProduct usbProduct = await _context.UsbProducts.FindAsync(id);

            if(usbProduct == null)
            {
                return NotFound();
            }

            ViewData["VendorId"] = new SelectList(_context.UsbVendors, "Id", "Id", usbProduct.VendorId);

            return View(usbProduct);
        }

        // POST: Admin/UsbProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Product,AddedWhen,ModifiedWhen,VendorId")]
                                              UsbProduct usbProduct)
        {
            if(id != usbProduct.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(usbProduct);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!UsbProductExists(usbProduct.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["VendorId"] = new SelectList(_context.UsbVendors, "Id", "Id", usbProduct.VendorId);

            return View(usbProduct);
        }

        // GET: Admin/UsbProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            UsbProduct usbProduct =
                await _context.UsbProducts.Include(u => u.Vendor).FirstOrDefaultAsync(m => m.Id == id);

            if(usbProduct == null)
            {
                return NotFound();
            }

            return View(usbProduct);
        }

        // POST: Admin/UsbProducts/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            UsbProduct usbProduct = await _context.UsbProducts.FindAsync(id);
            _context.UsbProducts.Remove(usbProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool UsbProductExists(int id) => _context.UsbProducts.Any(e => e.Id == id);
    }
}