using MotoDeliveryManager.Domain.Models;

namespace MotoDeliveryManager.Domain.Interfaces.Repositories
{
    public interface INotificacaoRepository
    {
        Task AddAsync(Notificacao notificacao);
        Task<List<Notificacao>> GetNotificacoesPorEntregadorAsync(int entregadorId);
        Task<List<Notificacao>> GetNotificacoesPorPedidoAsync(int pedidoId);
    }
}