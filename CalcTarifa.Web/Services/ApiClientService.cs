using System.Text;
using System.Text.Json;
using CalcTarifa.BusinessApplication.DTOs;

namespace CalcTarifa.Web.Services
{
    public interface IApiClientService
    {
        Task<CalcularEnvioResponse?>                 CalcularAsync(CalcularEnvioRequest request);
        Task<IEnumerable<TarifaRegionResponse>>      ObtenerTarifasAsync();
        Task<IEnumerable<HistorialCalculoResponse>>  ObtenerHistorialAsync(int cantidad = 5);
        Task<bool>                                   ActualizarTarifaAsync(int region, decimal nuevaTarifa);
        Task<bool>                                   EliminarTarifaAsync(int region);
        
        Task<(bool Succeeded, string? Token, string? Nombre, IEnumerable<string>? Roles, string? Error)> LoginAsync(string email, string password);
        Task<(bool Succeeded, string? Token, string? Nombre, IEnumerable<string>? Roles, IEnumerable<string>? Errors)> RegistroAsync(string email, string nombreCompleto, string password, string confirmPassword, int idRol);
        
        void SetToken(string? token);
    }

    public class ApiClientService : IApiClientService
    {
        private readonly HttpClient        _http;
        private readonly JsonSerializerOptions _opts =
            new() { PropertyNameCaseInsensitive = true };

        public ApiClientService(HttpClient http) => _http = http;

        public void SetToken(string? token)
        {
            _http.DefaultRequestHeaders.Authorization = 
                string.IsNullOrEmpty(token) ? null : new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<CalcularEnvioResponse?> CalcularAsync(CalcularEnvioRequest request)
        {
            var content  = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("api/envios/calcular", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                // Intentar extraer "detail" o "mensaje" si es un ProblemDetails
                try { 
                    using var doc = JsonDocument.Parse(errorMsg);
                    if (doc.RootElement.TryGetProperty("detail", out var detail)) errorMsg = detail.GetString();
                    else if (doc.RootElement.TryGetProperty("mensaje", out var mensaje)) errorMsg = mensaje.GetString();
                } catch { }

                throw new HttpRequestException(errorMsg, null, response.StatusCode);
            }
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CalcularEnvioResponse>(json, _opts);
        }

        public async Task<IEnumerable<TarifaRegionResponse>> ObtenerTarifasAsync()
        {
            var json = await _http.GetStringAsync("api/tarifas");
            return JsonSerializer.Deserialize<IEnumerable<TarifaRegionResponse>>(json, _opts)
                   ?? [];
        }

        public async Task<IEnumerable<HistorialCalculoResponse>> ObtenerHistorialAsync(int cantidad = 5)
        {
            var response = await _http.GetAsync($"api/envios/historial?cantidad={cantidad}");
            if (!response.IsSuccessStatusCode) return [];
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<HistorialCalculoResponse>>(json, _opts)
                   ?? [];
        }

        public async Task<bool> ActualizarTarifaAsync(int region, decimal nuevaTarifa)
        {
            var content = new StringContent(JsonSerializer.Serialize(nuevaTarifa), Encoding.UTF8, "application/json");
            var response = await _http.PutAsync($"api/tarifas/{region}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarTarifaAsync(int region)
        {
            var response = await _http.DeleteAsync($"api/tarifas/{region}");
            return response.IsSuccessStatusCode;
        }

        public async Task<(bool Succeeded, string? Token, string? Nombre, IEnumerable<string>? Roles, string? Error)> LoginAsync(string email, string password)
        {
            var body = new { Email = email, Password = password };
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("api/auth/login", content);
            
            var json = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonDocument.Parse(json);
                var user   = result.RootElement.GetProperty("user");
                var roles  = user.GetProperty("roles").EnumerateArray().Select(r => r.GetString() ?? "").ToList();
                
                return (true, 
                        result.RootElement.GetProperty("token").GetString(), 
                        user.GetProperty("nombre").GetString(), 
                        roles,
                        null);
            }
            
            var error = "Error al iniciar sesión.";
            try { error = JsonDocument.Parse(json).RootElement.GetProperty("mensaje").GetString(); } catch { }
            return (false, null, null, null, error);
        }

        public async Task<(bool Succeeded, string? Token, string? Nombre, IEnumerable<string>? Roles, IEnumerable<string>? Errors)> RegistroAsync(string email, string nombreCompleto, string password, string confirmPassword, int idRol)
        {
            var body = new { Email = email, NombreCompleto = nombreCompleto, Password = password, ConfirmarPassword = confirmPassword, Rol = (CalcTarifa.Domain.Enums.RolUsuario)idRol };
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("api/auth/registro", content);
            
            var json = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonDocument.Parse(json);
                var user   = result.RootElement.GetProperty("user");
                var roles  = user.GetProperty("roles").EnumerateArray().Select(r => r.GetString() ?? "").ToList();

                return (true, 
                        result.RootElement.GetProperty("token").GetString(), 
                        user.GetProperty("nombre").GetString(), 
                        roles,
                        null);
            }
            
            try 
            { 
                var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                if (root.TryGetProperty("errores", out var err) || root.TryGetProperty("Errores", out err))
                {
                    var errors = err.EnumerateArray().Select(e => e.GetString() ?? "").ToList();
                    return (false, null, null, null, errors);
                }
                
                if (root.TryGetProperty("mensaje", out var msg) || root.TryGetProperty("Mensaje", out msg))
                {
                    return (false, null, null, null, [msg.GetString() ?? "Error desconocido"]);
                }

                return (false, null, null, null, ["Error en el servidor."]);
            }
 
            catch 
            { 
                return (false, null, null, null, ["Error en el registro."]); 
            }
        }
    }
}
