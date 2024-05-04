using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KursovaDBFinal.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KursovaDBFinal.Models;
using Microsoft.AspNetCore.Authorization;

namespace KursovaDBFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class InventoryLocationController : ControllerBase
    {
        private readonly HouseholdAppliancesContext _context;

        public InventoryLocationController(HouseholdAppliancesContext context)
        {
            _context = context;
        }

        // GET: api/InventoryLocation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryLocation>>> GetInventoryLocations()
        {
            await Logger.Log(User?.Identity?.Name ?? "User", "Viewed", "Inventory Locations", DateTime.UtcNow);
            return await _context.InventoryLocations.ToListAsync();
        }

        // GET: api/InventoryLocation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryLocation>> GetInventoryLocation(int id)
        {
            var inventoryLocation = await _context.InventoryLocations.FindAsync(id);

            if (inventoryLocation == null)
            {
                return NotFound();
            }
            
            await Logger.Log(User?.Identity?.Name ?? "User", "Viewed", $"Inventory Location with id {id}", DateTime.UtcNow);
            return inventoryLocation;
        }
        

        // POST: api/InventoryLocation
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InventoryLocation>> PostInventoryLocation(InventoryLocation inventoryLocation)
        {
            _context.InventoryLocations.Add(inventoryLocation);
            await _context.SaveChangesAsync();

            await Logger.Log(User?.Identity?.Name ?? "User", "Created", "Inventory Location", DateTime.UtcNow);
            return CreatedAtAction("GetInventoryLocation", new { id = inventoryLocation.InventoryLocationId }, inventoryLocation);
        }

        // DELETE: api/InventoryLocation/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryLocation(int id)
        {
            var inventoryLocation = await _context.InventoryLocations.FindAsync(id);
            if (inventoryLocation == null)
            {
                return NotFound();
            }

            _context.InventoryLocations.Remove(inventoryLocation);
            await _context.SaveChangesAsync();

            await Logger.Log(User?.Identity?.Name ?? "User", "Deleted", $"Inventory Location with id {id}", DateTime.Now);
            return NoContent();
        }
    }
}
