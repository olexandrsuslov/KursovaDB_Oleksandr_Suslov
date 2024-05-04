using KursovaDBFinal.Loggers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KursovaDBFinal.Models;
using X.PagedList;

namespace KursovaDBFinal.Controllers
{
    public class ApplianceController : Controller
    {
        private readonly HouseholdAppliancesContext _context;
        
        public ApplianceController(HouseholdAppliancesContext context)
        {
            _context = context;
        }
        
        // GET: Appliance
        public async Task<IActionResult> Index(int? page, string currentFilter, string searchString)
        {
            const int pageSize = 16;
            var appliances = _context.Appliances.AsQueryable(); // Get initial queryable
            
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            // Apply search filter if searchString is not null or empty
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower(); // Convert search string to lowercase
        
                appliances = appliances.Include(p => p.Category)
                    .Where(s => s.Name.ToLower().Contains(searchString)
                                || s.Category.Name.ToLower().Contains(searchString));
            }
            else
            {
                // If no search string, just include categories
                appliances = appliances.Include(p => p.Category);
            }

            await Logger.Log(User?.Identity?.Name ?? "User", "Viewed", "Appliances", DateTime.UtcNow);
    
            return View(await appliances.ToPagedListAsync(page ?? 1, pageSize));
        }



        // GET: Appliance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Appliances
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ApplianceId == id);
            if (product == null)
            {
                return NotFound();
            }

            await Logger.Log(User?.Identity?.Name ?? "User", "Viewed", $"Appliance with id {id}", DateTime.UtcNow);
            return View(product);
        }

        // GET: Appliance/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.ApplianceCategories, "CategoryId", "Name");
            return View();
        }

        // POST: Appliance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApplianceId,Name,Description,CategoryId,Price,DateAdded,AvgRating,PhotoUrl")] Appliance product)
        {
            try
            {
                product.Category = await _context.ApplianceCategories.FindAsync(product.CategoryId);
                ModelState.Remove("Category");

                if (ModelState.IsValid)
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                ViewData["CategoryId"] =
                    new SelectList(_context.ApplianceCategories, "CategoryId", "Name", product.Category.Name);
                await Logger.Log(User?.Identity?.Name ?? "User", "Created", "Appliance", DateTime.UtcNow);
                return View(product);
            }
            catch (DbUpdateException ex)
            {
                // Log the exception or handle it as needed
                var message = ex.InnerException.Message;
                if (ex.InnerException.Message.Contains("avg_ratingLimits"))
                {
                    message = "Appliance average rating should be between 0 and 5";
                }

                ModelState.AddModelError("", message);
                ViewData["CategoryId"] =
                    new SelectList(_context.ApplianceCategories, "CategoryId", "Name", product.Category.Name);
                return View(product); // You can return the view with an error message or redirect to an error page
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                var message = ex.InnerException.Message;
                ModelState.AddModelError("", message);
                ViewData["CategoryId"] =
                    new SelectList(_context.ApplianceCategories, "CategoryId", "Name", product.Category.Name);
                return View(product); // You can return the view with an error message or redirect to an error page
            }
        }

        // GET: Appliance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Appliances.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            product.Category = await _context.ApplianceCategories.FindAsync(product.CategoryId);
            ViewData["CategoryId"] = new SelectList(_context.ApplianceCategories, "CategoryId", "Name", product.Category.Name);
            return View(product);
        }

        // POST: Appliance/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApplianceId,Name,Description,CategoryId,Price,DateAdded,AvgRating,PhotoUrl")] Appliance product)
        {
            if (id != product.ApplianceId)
            {
                return NotFound();
            }

            product.Category = await _context.ApplianceCategories.FindAsync(product.CategoryId);
            ModelState.Remove("Category");
    
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    await Logger.Log(User?.Identity?.Name ?? "User", "Edited", $"Appliance with id {id}", DateTime.UtcNow);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplianceExists(product.ApplianceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Concurrency error: The record you attempted to edit was modified by another user. Please refresh the page and try again.");
                        return View(product);
                    }
                }
                catch (DbUpdateException ex)
                {
                    var message = ex.InnerException.Message;
                    if (ex.InnerException.Message.Contains("avg_ratingLimits"))
                    {
                        message = "Appliance average rating should be between 0 and 5";
                    }
                    ModelState.AddModelError("", message);
                    ViewData["CategoryId"] = new SelectList(_context.ApplianceCategories, "CategoryId", "Name", product.Category.Name);
                    return View(product); // You can return the view with an error message or redirect to an error page
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    var message = ex.InnerException.Message;
                    ModelState.AddModelError("", message);
                    ViewData["CategoryId"] =
                        new SelectList(_context.ApplianceCategories, "CategoryId", "Name", product.Category.Name);
                    return View(product); // You can return the view with an error message or redirect to an error page
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.ApplianceCategories, "CategoryId", "Name", product.Category.Name);
            return View(product);
        }


        // GET: Appliance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Appliances
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ApplianceId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Appliance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Appliances.FindAsync(id);
            if (product != null)
            {
                _context.Appliances.Remove(product);
            }
            await _context.SaveChangesAsync();
            await Logger.Log(User?.Identity?.Name ?? "User", "Deleted", $"Appliance with id {id}", DateTime.Now);
            return RedirectToAction(nameof(Index));
        }

        private bool ApplianceExists(int id)
        {
            return _context.Appliances.Any(e => e.ApplianceId == id);
        }
    }
}
