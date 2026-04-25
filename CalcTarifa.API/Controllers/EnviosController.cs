using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CalcTarifa.BusinessApplication.DTOs;
using CalcTarifa.BusinessApplication.Interfaces.UseCases;

namespace CalcTarifa.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EnviosController : ControllerBase
    {
        private readonly ICalcularEnvioUseCase            _calcular;
        private readonly IConsultarTarifasUseCase         _tarifas;
        private readonly IObtenerUltimosHistorialUseCase  _ultimosHistorial;
        private readonly IObtenerHistorialPorEmailUseCase _historialPorEmail;

        public EnviosController(
            ICalcularEnvioUseCase            calcular,
            IConsultarTarifasUseCase         tarifas,
            IObtenerUltimosHistorialUseCase  ultimosHistorial,
            IObtenerHistorialPorEmailUseCase historialPorEmail)
        {
            _calcular          = calcular;
            _tarifas           = tarifas;
            _ultimosHistorial  = ultimosHistorial;
            _historialPorEmail = historialPorEmail;
        }

        // calcula costo y registra consulta
        [HttpPost("calcular")]
        [ProducesResponseType(typeof(CalcularEnvioResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Calcular([FromBody] CalcularEnvioRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Si el usuario está autenticado, asociamos el UserId
            if (User.Identity?.IsAuthenticated == true)
            {
                request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var resultado = await _calcular.EjecutarAsync(request);
            return Ok(resultado);
        }


        // retorna ultimos n calculos
        [HttpGet("historial")]
        [ProducesResponseType(typeof(IEnumerable<HistorialCalculoResponse>), 200)]
        public async Task<IActionResult> ObtenerHistorial([FromQuery] int cantidad = 10)
        {
            string? userId = null;
            if (User.Identity?.IsAuthenticated == true)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var historial = await _ultimosHistorial.EjecutarAsync(cantidad, userId);
            return Ok(historial);
        }

        // historial filtrado por email
        [HttpGet("historial/cliente")]
        [ProducesResponseType(typeof(IEnumerable<HistorialCalculoResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ObtenerPorEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new { error = "El email es obligatorio." });

            var historial = await _historialPorEmail.EjecutarAsync(email);
            return Ok(historial);
        }
    }
}
