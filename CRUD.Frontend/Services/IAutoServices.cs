using CRUD.Shared;

namespace CRUD.Frontend.Services;

public interface IAutoServices
{
    Task<List<Auto>> Consultar(string? filtro = null);
    Task<Auto> Crear(Auto entity);
    Task<Auto> Modificar(int id, Auto entity);
    Task<bool> Eliminar(int id);
}
