using MotoDeliveryManager.Domain.Models;

namespace MotoDeliveryManager.Domain.Interfaces.Services
{
    public interface IMotoService
    {
        Task<List<Moto>> GetAllMotosAsync();
        Task<List<Moto>> GetMotosByPlacaAsync(string placa);
        Task<Moto> GetMotoByIdAsync(int id);
        Task AddMotoAsync(Moto moto);
        Task UpdateMotoAsync(int id, Moto moto);
        Task RemoveMotoAsync(int id);
    }
}
