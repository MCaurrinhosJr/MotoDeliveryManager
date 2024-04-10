using MotoDeliveryManager.Domain.Models;

namespace MotoDeliveryManager.Domain.Interfaces.Services
{
    public interface IEntregadorService
    {
        Task<List<Entregador>> GetAllEntregadoresAsync();
        Task<Entregador> GetEntregadorByIdAsync(int id);
        Task AddEntregadorAsync(Entregador entregador);
        Task UpdateEntregadorAsync(int id, Entregador entregador);
        Task RemoveEntregadorAsync(int id);
    }
}
