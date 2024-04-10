using MotoDeliveryManager.Domain.Models;

namespace MotoDeliveryManager.Domain.Interfaces.Repositories
{
    public interface IPedidoRepository
    {
        Task<List<Pedido>> GetAllAsync();
        Task<Pedido> GetByIdAsync(int id);
        Task<Pedido> AddAsync(Pedido pedido);
        Task<Pedido> UpdateAsync(Pedido pedido);
        Task RemoveAsync(int id);
    }
}
