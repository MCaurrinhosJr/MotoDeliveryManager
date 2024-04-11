using Microsoft.EntityFrameworkCore;
using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Models;
using MotoDeliveryManager.Infra.Context;

namespace MotoDeliveryManager.Infra.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly MDMDbContext _context;

        public PedidoRepository(MDMDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pedido>> GetAllAsync()
        {
            return await _context.Pedidos.ToListAsync();
        }

        public async Task<Pedido> GetByIdAsync(int id)
        {
            return await _context.Pedidos.FindAsync(id);
        }

        public async Task<Pedido> AddAsync(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<Pedido> UpdateAsync(Pedido pedido)
        {
            _context.Entry(pedido).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task RemoveAsync(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Pedido>> GetPedidosByEntregadorIdAsync(int id)
        {
            return await _context.Pedidos.Where(p => p.EntregadorId == id).ToListAsync();
        }
    }
}
