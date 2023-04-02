using ConcertDB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ConcertDB.Controllers
{
    public class TicketController : Controller

    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Tickets/Use/5
        public async Task<IActionResult> Use(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            ViewBag.EntranceGates = new SelectList(new[]
            {
            new { Value = "Norte", Text = "Norte" },
            new { Value = "Sur", Text = "Sur" },
            new { Value = "Oriental", Text = "Oriental" },
            new { Value = "Occidental", Text = "Occidental" }
        }, "Value", "Text");

            return View(ticket);
        }

    }
}