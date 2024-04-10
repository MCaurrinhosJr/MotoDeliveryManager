using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Interfaces.Services;
using MotoDeliveryManager.Domain.Models;

namespace MotoDeliveryManager.Domain.Services
{
    public class MotoService : IMotoService
    {
        private readonly IMotoRepository _motoRepository;

        public MotoService(IMotoRepository motoRepository)
        {
            _motoRepository = motoRepository;
        }

        public async Task<List<Moto>> GetAllMotosAsync()
        {
            return await _motoRepository.GetAllAsync();
        }

        public async Task<List<Moto>> GetMotosByPlacaAsync(string placa)
        {
            return await _motoRepository.GetByPlacaAsync(placa);
        }

        public async Task<Moto> GetMotoByIdAsync(int id)
        {
            return await _motoRepository.GetByIdAsync(id);
        }

        public async Task AddMotoAsync(Moto newMoto)
        {
            if (newMoto == null)
                throw new ArgumentNullException(nameof(newMoto), "Dados da moto não foram fornecidos.");

            if (string.IsNullOrWhiteSpace(newMoto.Ano))
                throw new ArgumentException("Ano da moto é obrigatório.", nameof(newMoto.Ano));

            if (string.IsNullOrWhiteSpace(newMoto.Modelo))
                throw new ArgumentException("Modelo da moto é obrigatório.", nameof(newMoto.Modelo));

            if (string.IsNullOrWhiteSpace(newMoto.Placa))
                throw new ArgumentException("Placa da moto é obrigatória.", nameof(newMoto.Placa));

            var motosByPlaca = await _motoRepository.GetByPlacaAsync(newMoto.Placa);
            if (motosByPlaca.Count > 0)
                throw new ArgumentException("Já existe uma moto cadastrada com essa placa.", nameof(newMoto.Placa));

            await _motoRepository.AddAsync(newMoto);
        }

        public async Task UpdateMotoAsync(int id, Moto moto)
        {
            var existingMoto = await _motoRepository.GetByIdAsync(id);
            if (existingMoto == null)
                throw new KeyNotFoundException($"Moto com ID {id} não encontrada.");

            var existingMotos = await _motoRepository.GetByPlacaAsync(moto.Placa);
            if (existingMotos.Any())
            {
                throw new InvalidOperationException("Já existe uma moto cadastrada com essa nova placa.");
            }

            await _motoRepository.UpdateAsync(moto);
        }

        public async Task RemoveMotoAsync(int id)
        {
            var moto = await _motoRepository.GetByIdAsync(id);
            if (moto == null)
                throw new KeyNotFoundException($"Moto com ID {id} não encontrada.");

            if (moto.Locacoes != null && moto.Locacoes.Any())
                throw new InvalidOperationException("Não é possível remover uma moto com registros de locações associados.");

            await _motoRepository.RemoveAsync(id);
        }
    }
}