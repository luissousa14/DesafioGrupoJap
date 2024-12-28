using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrupoJap.Data;
using GrupoJap.Models;

namespace GrupoJap.Controllers
{
    public class VeiculosController : Controller
    {
        private readonly AppDbContext _context;

        public VeiculosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Veiculo.Include(x => x.TipoCombustivel).ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculo
                .Include(x => x.TipoCombustivel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        public async Task<IActionResult> Create()
        {
            #region DropDowns
            ViewBag.TiposCombustivel = await GetTiposCombustivelAsync();
            #endregion

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Veiculo veiculo)
        {
            var matriculaExiste = await _context.Veiculo
                .Where(c => c.Matricula == veiculo.Matricula && c.Id != veiculo.Id)
                .FirstOrDefaultAsync();

            #region Validações
            if (matriculaExiste != null)
            {
                ModelState.AddModelError("Matricula", "A matricula já está a ser utilizada em outro veiculo.");
            }

            if (!await _context.TipoCombustivel.AnyAsync(x => x.Id == veiculo.TipoCombustivelId))
            {
                ModelState.AddModelError("TipoCombustivelId", "O tipo de combustivel não existe.");
            }

            if (veiculo.AnoFabrico > DateTime.Now.Year)
            {
                ModelState.AddModelError("AnoFabrico", "O ano de fabrico não pode ser posterior ao ano atual.");
            }
            #endregion

            #region DropDowns
            ViewBag.TiposCombustivel = await GetTiposCombustivelAsync();
            #endregion

            if (ModelState.IsValid)
            {
                veiculo.Id = Guid.NewGuid();
                _context.Add(veiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(veiculo);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculo.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }

            #region DropDowns
            ViewBag.TiposCombustivel = await GetTiposCombustivelAsync();
            #endregion

            return View(veiculo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, Veiculo veiculo)
        {
            if (id != veiculo.Id)
            {
                return NotFound();
            }

            var matriculaExiste = await _context.Veiculo
                .Where(c => c.Matricula == veiculo.Matricula && c.Id != veiculo.Id)
                .FirstOrDefaultAsync();

            #region Validações
            if (matriculaExiste != null)
            {
                ModelState.AddModelError("Matricula", "A matricula já está a ser utilizada em outro veiculo.");
            }

            if (!await _context.TipoCombustivel.AnyAsync(x => x.Id == veiculo.TipoCombustivelId))
            {
                ModelState.AddModelError("TipoCombustivelId", "O tipo de combustivel não existe.");
            }

            if (veiculo.AnoFabrico > DateTime.Now.Year)
            {
                ModelState.AddModelError("AnoFabrico", "O ano de fabrico não pode ser posterior ao ano atual.");
            }
            #endregion

            #region DropDowns
            ViewBag.TiposCombustivel = await GetTiposCombustivelAsync();
            #endregion

            if (ModelState.IsValid)
            {
                _context.Veiculo.Update(veiculo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(veiculo);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var veiculo = await _context.Veiculo
                .Include(v => v.Contratos) // Carrega os contratos associados
                .FirstOrDefaultAsync(v => v.Id == id);

            if (veiculo == null)
            {
                return NotFound();
            }

            #region Validações
            if (veiculo.Contratos.Any())
            {
                TempData["ErrorMessage"] = "O veículo não pode ser excluído porque está associado a contratos de aluguer.";
                return RedirectToAction(nameof(Delete));
            }
            #endregion

            _context.Veiculo.Remove(veiculo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetTiposCombustivelAsync()
        {
            return await _context.TipoCombustivel
                .Select(tc => new SelectListItem
                {
                    Value = tc.Id.ToString(),
                    Text = tc.Descritivo
                }).ToListAsync();
        }

        private bool VeiculoExists(Guid id)
        {
            return _context.Veiculo.Any(e => e.Id == id);
        }
    }
}
