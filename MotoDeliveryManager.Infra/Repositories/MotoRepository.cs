using Microsoft.EntityFrameworkCore;
using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Models;
using MotoDeliveryManager.Infra.Context;

namespace MotoDeliveryManager.Infra.Repositories
{
    public class MotoRepository : IMotoRepository
    {
        private readonly MDMDbContext _context;

        public MotoRepository(MDMDbContext context)
        {
            _context = context;
        }

        public async Task<List<Moto>> GetAllAsync()
        {
            return await _context.Motos.ToListAsync();
        }

        public async Task<List<Moto>> GetByPlacaAsync(string placa)
        {
            return await _context.Motos.Where(m => m.Placa == placa).ToListAsync();
        }

        public async Task<Moto> GetByIdAsync(int id)
        {
            return await _context.Motos.FindAsync(id);
        }

        public async Task AddAsync(Moto moto)
        {
            await _context.Motos.AddAsync(moto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Moto moto)
        {
            _context.Entry(moto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto != null)
            {
                _context.Motos.Remove(moto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
