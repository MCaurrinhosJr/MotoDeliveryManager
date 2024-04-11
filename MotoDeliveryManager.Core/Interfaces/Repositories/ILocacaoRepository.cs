using MotoDeliveryManager.Domain.Models;

namespace MotoDeliveryManager.Domain.Interfaces.Repositories
{
    public interface ILocacaoRepository
    {
        Task<List<Locacao>> GetAllAsync();
        Task<Locacao> GetByIdAsync(int id);
        Task<Locacao> AddAsync(Locacao locacao);
        Task UpdateAsync(Locacao locacao);
        Task RemoveAsync(int id);
    }
}