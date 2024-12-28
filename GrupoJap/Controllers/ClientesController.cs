using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrupoJap.Models;
using GrupoJap.Data;

namespace GrupoJap.Controllers
{
    public class ClientesController : Controller
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Cliente.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {

            #region Validações
            var emailExistente = await _context.Cliente
                .Where(c => c.Email == cliente.Email && c.Id != cliente.Id)
                .FirstOrDefaultAsync();

            if (emailExistente != null)
            {
                ModelState.AddModelError("Email", "O email já está a ser utilizado em outro cliente.");
            }
            #endregion

            if (ModelState.IsValid)
            {
                cliente.Id = Guid.NewGuid();
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            #region Validações
            var emailExistente = await _context.Cliente
                .Where(c => c.Email == cliente.Email && c.Id != cliente.Id)
                .FirstOrDefaultAsync();

            if (emailExistente != null)
            {
                ModelState.AddModelError("Email", "O email já está a ser utilizado em outro cliente.");
            }
            #endregion

            if (ModelState.IsValid)
            {
                _context.Cliente.Update(cliente);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cliente = await _context.Cliente
                .Include(v => v.Contratos)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (cliente == null)
            {
                return NotFound();
            }

            #region Validações
            if (cliente.Contratos.Any())
            {
                TempData["ErrorMessage"] = "O cliente não pode ser excluído porque está associado a contratos de aluguer.";
                return RedirectToAction(nameof(Delete));
            }
            #endregion

            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(Guid id)
        {
            return _context.Cliente.Any(e => e.Id == id);
        }
    }
}
