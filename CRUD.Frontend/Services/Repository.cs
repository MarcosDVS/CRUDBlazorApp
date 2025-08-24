using System.Text.Json;

namespace CRUD.Frontend.Services;

// Clase que implementa un repositorio genérico para consumir una API REST.
// Sirve para centralizar todas las operaciones CRUD (Create, Read, Update, Delete)
// utilizando HttpClient para comunicarse con el backend.
public class Repository : IRepository
{
    private readonly HttpClient _httpClient;

    // Constructor: recibe una instancia de HttpClient inyectada por dependencia.
    // HttpClient se encarga de hacer las peticiones HTTP hacia la API.
    public Repository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Opciones de serialización JSON.
    // Se configura para que NO importe si los nombres de las propiedades 
    // empiezan en mayúscula o minúscula (PropertyNameCaseInsensitive = true).
    private JsonSerializerOptions _jsonDefaultOptions => new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };

    // Método DELETE: elimina un recurso en la API según su ID.
    // url → endpoint de la API (ej: "api/productos/5").
    public async Task<object> DeleteAsync(string url, int id)
    {
        var response = await _httpClient.DeleteAsync(url); // Hace la petición DELETE.
        response.EnsureSuccessStatusCode(); // Lanza excepción si falla.
        return await response.Content.ReadAsStringAsync(); // Devuelve la respuesta como string.
    }

    // Método GET: obtiene una lista o un objeto desde la API.
    // Se deserializa la respuesta JSON al tipo genérico T.
    public async Task<T> GetAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url); // Hace la petición GET.
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(); // Lee respuesta como texto.

        // Convierte el JSON recibido en un objeto de tipo T (ej: List<Producto>).
        return JsonSerializer.Deserialize<T>(content, _jsonDefaultOptions)
               ?? throw new InvalidOperationException("Deserialization failed.");
    }

    // Método GET por ID: obtiene un recurso específico usando su ID.
    public async Task<T> GetByIdAsync<T>(string url, int id)
    {
        var requestUrl = $"{url}/{id}"; // Construye la URL completa (ej: api/productos/1).
        var response = await _httpClient.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(content, _jsonDefaultOptions)
               ?? throw new InvalidOperationException("Deserialization failed.");
    }

    // Método POST: envía un nuevo recurso a la API para crearlo.
    public async Task<object> PostAsync<T>(string url, T entity)
    {
        // Serializa el objeto a JSON.
        var content = new StringContent(
            JsonSerializer.Serialize(entity),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(url, content); // Hace el POST.
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    // Método PUT: actualiza un recurso existente en la API.
    public async Task<object> PutAsync<T>(string url, T entity)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(entity),
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PutAsync(url, content); // Hace el PUT.
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
