using CRUD.Shared;
using System.Net.Http.Json;

namespace CRUD.Frontend.Services;

public class AutoServices : IAutoServices
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "api/autos"; // Ajusta si tu ruta del controlador es diferente


    // Constructor: recibe una instancia de HttpClient inyectada por dependencia.
    // HttpClient se encarga de hacer las peticiones HTTP hacia la API.
    public AutoServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // GET: Consultar autos (con o sin filtro)
    public async Task<List<Auto>> Consultar(string? filtro = null)
    {
        string url = BaseUrl;

        if (!string.IsNullOrWhiteSpace(filtro))
            url += $"?filtro={filtro}";

        var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<Auto>>>(url);

        return response?.Data ?? new List<Auto>();
    }

    // POST: Crear nuevo auto
    public async Task<Auto> Crear(Auto entity)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseUrl, entity);

        if (response.IsSuccessStatusCode)
        {
            var created = await response.Content.ReadFromJsonAsync<Auto>();
            return created!;
        }

        throw new Exception($"Error al crear Auto: {response.ReasonPhrase}");
    }

    // PUT: Modificar auto existente
    public async Task<Auto> Modificar(int id, Auto entity)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", entity);

        if (response.IsSuccessStatusCode)
        {
            var updated = await response.Content.ReadFromJsonAsync<Auto>();
            return updated!;
        }

        throw new Exception($"Error al modificar Auto: {response.ReasonPhrase}");
    }

    // DELETE: Eliminar auto por Id
    public async Task<bool> Eliminar(int id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        return response.IsSuccessStatusCode;
    }
}

