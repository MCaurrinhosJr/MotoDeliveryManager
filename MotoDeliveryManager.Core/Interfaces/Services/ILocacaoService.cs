using MotoDeliveryManager.Domain.Models;

namespace MotoDeliveryManager.Domain.Interfaces.Services
{
    public interface ILocacaoService
    {
        Task<List<Locacao>> GetAllLocacoesAsync();
        Task<Locacao> GetLocacaoByIdAsync(int id);
        Task<Locacao> AlugarMotoAsync(AluguelRequest request);
        Task DevolverMotoAsync(DevolucaoRequest request);
        Task<List<Locacao>> GetLocacoesByEntregadorIdAsync(int id);
    }
}