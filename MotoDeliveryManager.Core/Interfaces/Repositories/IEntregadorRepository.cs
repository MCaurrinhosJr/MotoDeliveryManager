using MotoDeliveryManager.Domain.Models;

namespace MotoDeliveryManager.Domain.Interfaces.Repositories
{
    public interface IEntregadorRepository
    {
        Task<List<Entregador>> GetAllAsync();
        Task<Entregador> GetByIdAsync(int id);
        Task<Entregador> GetByCnpjAsync(string cnpj);
        Task<Entregador> GetByNumeroCnhAsync(string numeroCnh);
        Task<Entregador> AddAsync(Entregador entregador);
        Task UpdateAsync(Entregador entregador);
        Task RemoveAsync(int id);
    }
}
