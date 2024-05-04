using KursovaDBFinal.Loggers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KursovaDBFinal.Models;

namespace KursovaDBFinal.Controllers
{
    public class OrderController : Controller
    {
        private readonly HouseholdAppliancesContext _context;

        public OrderController(HouseholdAppliancesContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var tablewareStoreContext = _context.Orders.Include(o => o.Customer).Include(o => o.ManagerUser).Include(o => o.Status);
            await Logger.Log(User?.Identity?.Name ?? "User", "Viewed", "Orders", DateTime.UtcNow);
            return View(await tablewareStoreContext.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.ManagerUser)
                .Include(o => o.Status)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            await Logger.Log(User?.Identity?.Name ?? "User", "Viewed", $"Order with id {id}", DateTime.UtcNow);
            return View(order);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["ManagerUserId"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "StatusId", "Name");
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CustomerId,OrderDate,TotalSum,ManagerUserId,StatusId")] Order order)
        {
            try
            {
                order.Status = await _context.OrderStatuses.FindAsync(order.StatusId);
                order.Customer = await _context.Customers.FindAsync(order.CustomerId);
                ModelState.Remove("Status");
                ModelState.Remove("Customer");
                if (ModelState.IsValid)
                {
                    _context.Add(order);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
                ViewData["ManagerUserId"] = new SelectList(_context.Users, "UserId", "UserId", order.ManagerUserId);
                ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "StatusId", "Name", order.Status.Name);
                await Logger.Log(User?.Identity?.Name ?? "User", "Created", "Order", DateTime.UtcNow);
                return View(order);
            }
            catch (DbUpdateException ex)
            {
                var message = ex.InnerException.Message;
                if (ex.InnerException.Message.Contains("overflow"))
                {
                    message = "Total sum value is too big";
                }
                ModelState.AddModelError("", message);
                ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
                ViewData["ManagerUserId"] = new SelectList(_context.Users, "UserId", "UserId", order.ManagerUserId);
                ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "StatusId", "Name", order.Status.Name);
                return View(order);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException.Message;
                ModelState.AddModelError("", message);
                ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
                ViewData["ManagerUserId"] = new SelectList(_context.Users, "UserId", "UserId", order.ManagerUserId);
                ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "StatusId", "Name", order.Status.Name);
                return View(order);
            }
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            order.Status = await _context.OrderStatuses.FindAsync(order.StatusId);
            order.Customer = await _context.Customers.FindAsync(order.CustomerId);

            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["ManagerUserId"] = new SelectList(_context.Users, "UserId", "UserId", order.ManagerUserId);
            ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "StatusId", "Name", order.Status.Name);
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,OrderDate,TotalSum,ManagerUserId,StatusId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }
            
            order.Status = await _context.OrderStatuses.FindAsync(order.StatusId);
            order.Customer = await _context.Customers.FindAsync(order.CustomerId);
            ModelState.Remove("Status");
            ModelState.Remove("Customer");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                    await Logger.Log(User?.Identity?.Name ?? "User", "Edited", $"Order with id {id}", DateTime.UtcNow);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException ex)
                {
                    var message = ex.InnerException.Message;
                    if (ex.InnerException.Message.Contains("overflow"))
                    {
                        message = "Total sum value is too big";
                    }
                    ModelState.AddModelError("", message);
                    ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
                    ViewData["ManagerUserId"] = new SelectList(_context.Users, "UserId", "UserId", order.ManagerUserId);
                    ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "StatusId", "Name", order.Status.Name);
                    return View(order);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["ManagerUserId"] = new SelectList(_context.Users, "UserId", "UserId", order.ManagerUserId);
            ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "StatusId", "Name", order.Status.Name);
            return View(order);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.ManagerUser)
                .Include(o => o.Status)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            await Logger.Log(User?.Identity?.Name ?? "User", "Deleted", $"Order with id {id}", DateTime.Now);
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
