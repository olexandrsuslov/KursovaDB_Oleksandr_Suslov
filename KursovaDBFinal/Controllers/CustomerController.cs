using KursovaDBFinal.Loggers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KursovaDBFinal.Models;
using Npgsql;

namespace KursovaDBFinal.Controllers
{
    public class CustomerController : Controller
    {
        private readonly HouseholdAppliancesContext _context;

        public CustomerController(HouseholdAppliancesContext context)
        {
            _context = context;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            var customers = _context.Customers.Include(c => c.User);
            await Logger.Log(User?.Identity?.Name ?? "User", "Viewed", "Customers", DateTime.Now);
            return View(await customers.ToListAsync());
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            await Logger.Log(User?.Identity?.Name ?? "User", "Viewed", $"Customer with id {id}", DateTime.Now);
            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FirstName,LastName,Phone,UserId")] Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", customer.UserId);
                await Logger.Log(User?.Identity?.Name ?? "User", "Created", "Customer", DateTime.Now);
                return View(customer);
            }
            catch (PostgresException ex)
            {
                var message = ex.InnerException.Message;
                if (ex.InnerException.Message.Contains("user_id_fk_uc"))
                {
                    message = "User with this ID already exists";
                }
                ModelState.AddModelError("", message);
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", customer.UserId);
                return View(customer);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                var message = ex.InnerException.Message;
                if (ex.InnerException.Message.Contains("user_id_fk_uc"))
                {
                    message = "There is already a customer with given user ID";
                }
                ModelState.AddModelError("", message);
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", customer.UserId);
                return View(customer);
            }
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", customer.UserId);
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FirstName,LastName,Phone,UserId")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                    await Logger.Log(User?.Identity?.Name ?? "User", "Edited", $"Customer with id {id}", DateTime.Now);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    var message = ex.InnerException.Message;
                    if (ex.InnerException.Message.Contains("user_id_fk_uc"))
                    {
                        message = "There is already a customer with given user ID";
                    }
                    ModelState.AddModelError("", message);
                    ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", customer.UserId);
                    return View(customer);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", customer.UserId);
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            await Logger.Log(User?.Identity?.Name ?? "User", "Deleted", $"Customer with id {id}", DateTime.Now);
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
