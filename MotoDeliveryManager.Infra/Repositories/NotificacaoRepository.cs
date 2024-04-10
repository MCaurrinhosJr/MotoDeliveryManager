using Microsoft.EntityFrameworkCore;
using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Models;
using MotoDeliveryManager.Infra.Context;

namespace MotoDeliveryManager.Infra.Repositories
{
    public class NotificacaoRepository : INotificacaoRepository
    {
        private readonly MDMDbContext _context;

        public NotificacaoRepository(MDMDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Notificacao notificacao)
        {
            _context.Notificacoes.Add(notificacao);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notificacao>> GetNotificacoesPorEntregadorAsync(int entregadorId)
        {
            return await _context.Notificacoes
                .Where(n => n.EntregadorId == entregadorId)
                .ToListAsync();
        }

        public async Task<List<Notificacao>> GetNotificacoesPorPedidoAsync(int pedidoId)
        {
            return await _context.Notificacoes
                .Where(n => n.PedidoId == pedidoId)
                .ToListAsync();
        }
    }
}