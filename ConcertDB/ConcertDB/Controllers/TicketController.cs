using System;
using ConcertDB.DAL.Entities;
using ConcertDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using ConcertDB.DAL;

namespace ConcertDB.Controllers
{
    public class TicketController : Controller
    {
        #region Constructor

        private readonly DatabaseContext _context;

        public TicketController(DatabaseContext context)
        {
            _context = context;
        }

        #endregion

        #region Private Methods

        private async Task<Ticket> GetTicketById(Guid? id)
        {
            Ticket ticket = await _context.Tickets
                .FirstOrDefaultAsync(ticket => ticket.Id == id);
            return ticket;
        }

        #endregion

        #region Ticket

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            return _context.Tickets != null ?
              View(await _context.Tickets.ToListAsync()) :
              Problem("Entity set 'DatabaseContext.Tickets'  is null.");
        }

        public async Task<IActionResult> Consultation()
        {
            return View();
        }

        // POST: Tickets/Validate
        [HttpPost]
        public ActionResult Validate(int ticketId)
        {
            var ticket = _context.Tickets.Find(ticketId);

            if (ticket == null)
            {
                ViewBag.ErrorMessage = "Boleta no válida";
            }
            else if (ticket.IsUsed)
            {
                ViewBag.ErrorMessage = "Boleta ya utilizada";
            }
            else
            {
                ticket.UseDate = DateTime.Now;
                ticket.IsUsed = true;
                ticket.EntranceGate = "Norte";          // TODO: Cambiar esto por la entrada real del usuario

                _context.SaveChanges();

                ViewBag.SuccessMessage = "Boleta validada correctamente";
            }

            return View("Index", _context.Tickets.ToList());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int id)
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

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, [Bind("EntranceGate")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ticket.UseDate = DateTime.Now;
                ticket.IsUsed = true;
                _context.Update(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Validate));
            }

            ViewBag.EntranceGates = new SelectList(new[]
            {
            new { Value = "Norte", Text = "Norte" },
            new { Value = "Sur", Text = "Sur" },
            new { Value = "Oriental", Text = "Oriental" },
            new { Value = "Occidental", Text = "Occidental" }
        }, "Value", "Text", ticket.EntranceGate);

            return View(ticket);
        }

        //Podemos crear El ValidarBoleta, para poder recibir los datos del formulario de 
        // validación de boletas y actualizar la info de la boleta en la base de datos
        // Condicional 2

        [HttpPost]
        public ActionResult ValidateTicket(string idTicket, string entrance)
        {
            // Verificar que el ID de la boleta es un número válido
            int id;
            if (!int.TryParse(idTicket, out id))
            {
                ViewBag.Mensaje = "Boleta no válida";
                return View("Index");
            }

            // Buscar la boleta en la base de datos
            Ticket ticket = _context.Tickets.Find(id);
            if (ticket == null)
            {
                ViewBag.Mensaje = "Boleta no válida";
                return View("Index");
            }

            // Verificar que la boleta no ha sido usada antes
            if (ticket.IsUsed)
            {
                ViewBag.Mensaje = "Boleta ya fue usada";
                return View("Index");
            }

            // Actualizar la información de la boleta en la base de datos
            ticket.IsUsed = true;
            ticket.UseDate = DateTime.Now;
            ticket.EntranceGate = entrance;
            _context.Entry(ticket).State = EntityState.Modified;
            _context.SaveChangesAsync();

            ViewBag.Mensaje = "Boleta validada";
            return View("Index");
        }

        // Para mostrar un mensaje en caso de que la boleta ya haya sido utilizada, 
        // debemos modificar la acción ValidarBoleta del controlador Home o principal. 
        // Podemos hacer esto de la siguiente manera: 
        // Condicional 3

        [HttpPost]
        public ActionResult ValidateTickets(string idTicket, string entrance)
        {
            // Buscamos la boleta en la base de datos
            var ticket = _context.Tickets.Find(idTicket);

            if (ticket == null)
            {
                // Si la boleta no existe, mostramos un mensaje de error
                ViewBag.Mensaje = "Boleta no válida";
            }
            else if (ticket.IsUsed)
            {
                // Si la boleta ya fue utilizada, mostramos un mensaje con la información de uso
                ViewBag.Mensaje = $"La boleta fue usada el {ticket.UseDate} en la portería {ticket.EntranceGate}";
            }
            else
            {
                // Si la boleta es válida, la marcamos como utilizada y guardamos la información de uso
                ticket.IsUsed = true;
                ticket.UseDate = DateTime.Now;
                ticket.EntranceGate = entrance;
                _context.SaveChangesAsync();

                ViewBag.Mensaje = "Boleta válida. Entrada autorizada.";
            }

            return View("Index");
        }
        #endregion
    }
}

