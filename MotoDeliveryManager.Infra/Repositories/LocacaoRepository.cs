using Microsoft.EntityFrameworkCore;
using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Models;
using MotoDeliveryManager.Infra.Context;

namespace MotoDeliveryManager.Infra.Repositories
{
    public class LocacaoRepository : ILocacaoRepository
    {
        private readonly MDMDbContext _context;

        public LocacaoRepository(MDMDbContext context)
        {
            _context = context;
        }

        public async Task<List<Locacao>> GetAllAsync()
        {
            return await _context.Locacoes.ToListAsync();
        }

        public async Task<Locacao> GetByIdAsync(int id)
        {
            return await _context.Locacoes.FindAsync(id);
        }

        public async Task<Locacao> AddAsync(Locacao locacao)
        {
            _context.Locacoes.Add(locacao);
            await _context.SaveChangesAsync();
            return locacao;
        }

        public async Task UpdateAsync(Locacao locacao)
        {
            _context.Entry(locacao).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var locacao = await _context.Locacoes.FindAsync(id);
            if (locacao != null)
            {
                _context.Locacoes.Remove(locacao);
                await _context.SaveChangesAsync();
            }
        }
    }
}