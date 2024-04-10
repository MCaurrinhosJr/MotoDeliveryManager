using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Interfaces.Services;
using MotoDeliveryManager.Domain.Models;

namespace MotoDeliveryManager.Domain.Services
{
    public class NotificacaoService : INotificacaoService
    {
        private readonly INotificacaoRepository _notificacaoRepository;
        private readonly IEntregadorRepository _entregadorRepository;

        public NotificacaoService(INotificacaoRepository notificacaoRepository, IEntregadorRepository entregadorRepository)
        {
            _notificacaoRepository = notificacaoRepository;
            _entregadorRepository = entregadorRepository;
        }

        public async Task EnviarNotificacaoAsync(int entregadorId, int pedidoId, string mensagem)
        {
            if (string.IsNullOrEmpty(mensagem))
            {
                throw new ArgumentException("A mensagem da notificação não pode ser vazia.");
            }

            var notificacao = new Notificacao
            {
                Mensagem = mensagem,
                DataEnvio = DateTime.Now,
                EntregadorId = entregadorId,
                PedidoId = pedidoId
            };

            await _notificacaoRepository.AddAsync(notificacao);
        }

        public async Task<List<Notificacao>> GetNotificacoesPorEntregadorAsync(int entregadorId)
        {
            return await _notificacaoRepository.GetNotificacoesPorEntregadorAsync(entregadorId);
        }

        public async Task<List<Entregador>> GetEntregadoresNotificadosAsync(int pedidoId)
        {
            var notificacoes = await _notificacaoRepository.GetNotificacoesPorPedidoAsync(pedidoId);
            var entregadores = new List<Entregador>();

            foreach (var notificacao in notificacoes)
            {
                var entregador = await _entregadorRepository.GetByIdAsync(notificacao.EntregadorId);
                if (entregador != null)
                {
                    entregadores.Add(entregador);
                }
            }

            return entregadores;
        }
    }
}