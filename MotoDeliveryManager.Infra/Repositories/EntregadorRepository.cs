using Microsoft.EntityFrameworkCore;
using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Models;
using MotoDeliveryManager.Infra.Context;

namespace MotoDeliveryManager.Infra.Repositories
{
    public class EntregadorRepository : IEntregadorRepository
    {
        private readonly MDMDbContext _context;

        public EntregadorRepository(MDMDbContext context)
        {
            _context = context;
        }

        public async Task<List<Entregador>> GetAllAsync()
        {
            return await _context.Entregadores.ToListAsync();
        }

        public async Task<Entregador> GetByIdAsync(int id)
        {
            return await _context.Entregadores.FindAsync(id);
        }

        public async Task<Entregador> GetByCnpjAsync(string cnpj)
        {
            return await _context.Entregadores.FindAsync(cnpj);
        }

        public async Task<Entregador> GetByNumeroCnhAsync(string numeroCnh)
        {
            return await _context.Entregadores.FindAsync(numeroCnh);
        }

        public async Task<Entregador> AddAsync(Entregador entregador)
        {
            _context.Entregadores.Add(entregador);
            await _context.SaveChangesAsync();
            return entregador;
            
        }

        public async Task UpdateAsync(Entregador entregador)
        {
            _context.Entry(entregador).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var entregador = await _context.Entregadores.FindAsync(id);
            if (entregador != null)
            {
                _context.Entregadores.Remove(entregador);
                await _context.SaveChangesAsync();
            }
        }
    }
}
