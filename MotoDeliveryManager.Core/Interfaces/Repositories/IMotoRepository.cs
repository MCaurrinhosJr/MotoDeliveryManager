using MotoDeliveryManager.Domain.Models;

namespace MotoDeliveryManager.Domain.Interfaces.Repositories
{
    public interface IMotoRepository
    {
        Task<List<Moto>> GetAllAsync();
        Task<List<Moto>> GetByPlacaAsync(string placa);
        Task<Moto> GetByIdAsync(int id);
        Task AddAsync(Moto moto);
        Task UpdateAsync(Moto moto);
        Task RemoveAsync(int id);
    }
}
