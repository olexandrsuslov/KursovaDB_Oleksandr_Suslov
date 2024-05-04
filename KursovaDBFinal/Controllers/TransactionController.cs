using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KursovaDBFinal.Loggers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KursovaDBFinal.Models;
using Microsoft.AspNetCore.Authorization;

namespace KursovaDBFinal.Controllers
{
    [Authorize(Roles = "Manager")]
    public class TransactionController : Controller
    {
        private readonly HouseholdAppliancesContext _context;

        public TransactionController(HouseholdAppliancesContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var householdAppliancesContext = _context.Transactions.Include(t => t.Order).Include(t => t.PaymentMethod);
            await Logger.Log(User?.Identity?.Name ?? "User", "Viewed", "Transactions", DateTime.UtcNow);
            return View(await householdAppliancesContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Order)
                .Include(t => t.PaymentMethod)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            await Logger.Log(User?.Identity?.Name ?? "User", "Viewed", $"Transaction with id {id}", DateTime.UtcNow);
            return View(transaction);
        }
        
        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Order)
                .Include(t => t.PaymentMethod)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            await Logger.Log(User?.Identity?.Name ?? "User", "Deleted", $"Transaction with id {id}", DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
        
    }
}
