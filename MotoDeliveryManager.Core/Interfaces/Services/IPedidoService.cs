using MotoDeliveryManager.Domain.Models;

namespace MotoDeliveryManager.Domain.Interfaces.Services
{
    public interface IPedidoService
    {
        Task<List<Pedido>> GetAllPedidosAsync();
        Task<Pedido> GetPedidoByIdAsync(int id);
        Task AddPedidoAsync(Pedido pedido);
        Task UpdatePedidoAsync(int id, Pedido pedido);
        Task EntregarPedidoAsync(int id, Pedido pedido, int entregadorId);
        Task AceitarPedidoAsync(int id, Pedido pedido, int entregadorId);
        Task RemovePedidoAsync(int id);
        Task<List<Pedido>> GetPedidosByEntregadorIdAsync(int id);
    }
}
