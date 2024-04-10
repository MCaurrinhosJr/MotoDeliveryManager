using MotoDeliveryManager.Domain.Models;

namespace MotoDeliveryManager.Domain.Interfaces.Services
{
    public interface INotificacaoService
    {
        Task EnviarNotificacaoAsync(int entregadorId, int pedidoId, string mensagem);
        Task<List<Notificacao>> GetNotificacoesPorEntregadorAsync(int entregadorId);
        Task<List<Entregador>> GetEntregadoresNotificadosAsync(int pedidoId);
    }
}