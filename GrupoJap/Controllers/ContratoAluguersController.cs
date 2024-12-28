using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrupoJap.Data;
using GrupoJap.Models;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace GrupoJap.Controllers
{
    public class ContratoAluguersController : Controller
    {
        private readonly AppDbContext _context;

        public ContratoAluguersController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            await UpdateEstadoVeiculos();
            var appDbContext = _context.ContratoAluguer.Include(c => c.Cliente).Include(c => c.Veiculo);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contratoAluguer = await _context.ContratoAluguer
                .Include(c => c.Cliente)
                .Include(c => c.Veiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contratoAluguer == null)
            {
                return NotFound();
            }

            return View(contratoAluguer);
        }

        public async Task<IActionResult> Create()
        {
            await UpdateEstadoVeiculos();

            var contratoAluguer = new ContratoAluguer
            {
                DataInicio = DateTime.Now.Date,
                DataFim = DateTime.Now.AddDays(7).Date
            };

            #region DropDowns
            ViewBag.Clientes = await GetClientesAsync();
            ViewBag.Veiculos = await GetVeiculosAsync();
            #endregion

            return View(contratoAluguer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContratoAluguer contratoAluguer)
        {
            await UpdateEstadoVeiculos();

            #region Validações
            if (!await _context.Cliente.AnyAsync(x => x.Id == contratoAluguer.ClienteId))
            {
                ModelState.AddModelError("ClienteId", "O cliente não existe.");
            }
            var veiculo = await _context.Veiculo.FindAsync(contratoAluguer.VeiculoId);
            var contratoExiste = await _context.ContratoAluguer
                  .AnyAsync(x => x.VeiculoId == contratoAluguer.VeiculoId &&
                  ((contratoAluguer.DataInicio < x.DataFim && contratoAluguer.DataFim > x.DataInicio) ||
                  (contratoAluguer.DataInicio == x.DataInicio || contratoAluguer.DataFim == x.DataFim)));
            if (veiculo == null)
            {
                ModelState.AddModelError("VeiculoId", "O veiculo não existe.");
            } else if (veiculo != null && (veiculo.Estado || contratoExiste))
            {
                ModelState.AddModelError("VeiculoId", "O veiculo já esta alugado para estas datas");
            }
            if (contratoAluguer.DataInicio < DateTime.Now.Date)
            {
                ModelState.AddModelError("DataInicio", "A data inicio aluguer não pode ser anterior à data atual.");
            }
            if (contratoAluguer.DataFim < contratoAluguer.DataInicio)
            {
                ModelState.AddModelError("DataFim", "A data fim aluguer deve ser posterior à data inicio aluguer");
            }
            #endregion

            if (ModelState.IsValid)
            {
                contratoAluguer.Id = Guid.NewGuid();
                _context.Add(contratoAluguer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            #region DropDowns
            ViewBag.Clientes = await GetClientesAsync();
            ViewBag.Veiculos = await GetVeiculosAsync();
            #endregion

            return View(contratoAluguer);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await UpdateEstadoVeiculos();

            var contratoAluguer = await _context.ContratoAluguer.FindAsync(id);
            if (contratoAluguer == null)
            {
                return NotFound();
            }

            #region DropDowns
            ViewBag.Clientes = await GetClientesAsync();
            ViewBag.Veiculos = await GetVeiculosAsync();
            #endregion

            return View(contratoAluguer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, ContratoAluguer contratoAluguer)
        {
            if (id != contratoAluguer.Id)
            {
                return NotFound();
            }

            await UpdateEstadoVeiculos();

            #region Validações
            if (!await _context.Cliente.AnyAsync(x => x.Id == contratoAluguer.ClienteId))
            {
                ModelState.AddModelError("ClienteId", "O cliente não existe.");
            }
            var veiculo = await _context.Veiculo.FindAsync(contratoAluguer.VeiculoId);
            var contratoExiste = await _context.ContratoAluguer
                  .AnyAsync(x => x.VeiculoId == contratoAluguer.VeiculoId &&
                  ((contratoAluguer.DataInicio < x.DataFim && contratoAluguer.DataFim > x.DataInicio) ||
                  (contratoAluguer.DataInicio == x.DataInicio || contratoAluguer.DataFim == x.DataFim)));
            if (veiculo == null)
            {
                ModelState.AddModelError("VeiculoId", "O veiculo não existe.");
            }
            else if (veiculo != null && (veiculo.Estado || contratoExiste) && veiculo.Id != contratoAluguer.VeiculoId)
            {
                ModelState.AddModelError("VeiculoId", "O veiculo já esta alugado");
            }
            if (contratoAluguer.DataInicio < DateTime.Now)
            {
                ModelState.AddModelError("DataInicio", "A data inicio aluguer não pode ser anterior à data atual.");
            }
            if (contratoAluguer.DataFim < contratoAluguer.DataInicio)
            {
                ModelState.AddModelError("DataFim", "A data fim aluguer deve ser posterior à data inicio aluguer");
            }
            #endregion

            if (ModelState.IsValid)
            {
                _context.Update(contratoAluguer);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            #region DropDowns
            ViewBag.Clientes = await GetClientesAsync();
            ViewBag.Veiculos = await GetVeiculosAsync();
            #endregion

            return View(contratoAluguer);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contratoAluguer = await _context.ContratoAluguer
                .Include(c => c.Cliente)
                .Include(c => c.Veiculo)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contratoAluguer == null)
            {
                return NotFound();
            }

            contratoAluguer.Veiculo.Estado = false;

            return View(contratoAluguer);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var contratoAluguer = await _context.ContratoAluguer.FindAsync(id);
            if (contratoAluguer != null)
            {
                _context.ContratoAluguer.Remove(contratoAluguer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetClientesAsync()
        {
            await UpdateEstadoVeiculos();

            return await _context.Cliente
                .Select(tc => new SelectListItem
                {
                    Value = tc.Id.ToString(),
                    Text = tc.NomeCompleto + '/' + tc.Email
                }).ToListAsync();
        }

        private async Task<IEnumerable<SelectListItem>> GetVeiculosAsync()
        {
            await UpdateEstadoVeiculos();

            return await _context.Veiculo.Where(x => x.Estado == false)
                .Select(tc => new SelectListItem
                {
                    Value = tc.Id.ToString(),
                    Text = tc.Marca + '/' + tc.Modelo + '-' + tc.Matricula
                }).ToListAsync();
        }

        private async Task UpdateEstadoVeiculos()
        {
            var contratosAluguer = await _context.ContratoAluguer.Include(x => x.Veiculo).Where(x => x.DataInicio <= DateTime.Now.Date).OrderBy(x => x.DataFim).ToListAsync();

            foreach (var item in contratosAluguer)
            {
                if (!item.Veiculo.Estado && item.DataInicio <= DateTime.Now.Date && item.DataFim >= DateTime.Now.Date)
                {
                    item.Veiculo.Estado = true;
                } else
                {
                    item.Veiculo.Estado = false;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
