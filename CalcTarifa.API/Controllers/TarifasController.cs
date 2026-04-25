using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CalcTarifa.BusinessApplication.Interfaces.Persistence;
using CalcTarifa.BusinessApplication.Interfaces.UseCases;
using CalcTarifa.BusinessApplication.DTOs;

namespace CalcTarifa.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarifasController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IConsultarTarifasUseCase _consultarUseCase;

        public TarifasController(IUnitOfWork uow, IConsultarTarifasUseCase consultarUseCase)
        {
            _uow = uow;
            _consultarUseCase = consultarUseCase;
        }

        // GET api/tarifas
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _consultarUseCase.EjecutarAsync();
            return Ok(result);
        }

        // PUT api/tarifas/{region}
        [HttpPut("{region}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int region, [FromBody] decimal nuevaTarifa)
        {
            var r = (CalcTarifa.Domain.Enums.RegionEnvio)region;
            var tarifa = await _uow.Tarifas.ObtenerPorRegionAsync(r);

            if (tarifa == null)
                return NotFound("Región no encontrada.");

            tarifa.ActualizarTarifa(nuevaTarifa);
            await _uow.CommitAsync();

            return Ok(new { Region = r.ToString(), NuevaTarifa = nuevaTarifa });
        }

        // DELETE api/tarifas/{region}
        [HttpDelete("{region}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int region)
        {
            var r = (CalcTarifa.Domain.Enums.RegionEnvio)region;
            var tarifa = await _uow.Tarifas.ObtenerPorRegionAsync(r);

            if (tarifa == null)
                return NotFound("Región no encontrada.");

            tarifa.Desactivar();
            await _uow.CommitAsync();

            return Ok(new { Message = $"La región {r} ha sido desactivada." });
        }
    }
}
