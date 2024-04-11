using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Interfaces.Services;
using MotoDeliveryManager.Domain.Models;
using MotoDeliveryManager.Domain.Models.Enum;
using MotoDeliveryManager.Domain.Services.FirebaseStorage;

namespace MotoDeliveryManager.Domain.Services
{
    public class EntregadorService : IEntregadorService
    {
        private readonly IEntregadorRepository _entregadorRepository;
        private readonly IFirebaseStorageService _firebaseStorageService;

        public EntregadorService(IEntregadorRepository entregadorRepository, IFirebaseStorageService firebaseStorageService)
        {
            _entregadorRepository = entregadorRepository;
            _firebaseStorageService = firebaseStorageService;
        }

        public async Task<List<Entregador>> GetAllEntregadoresAsync()
        {
            return await _entregadorRepository.GetAllAsync();
        }

        public async Task<Entregador> GetEntregadorByIdAsync(int id)
        {
            return await _entregadorRepository.GetByIdAsync(id);
        }

        public async Task AddEntregadorAsync(Entregador entregador)
        {
            ValidateEntregador(entregador);

            var existingCnpj = await _entregadorRepository.GetByCnpjAsync(entregador.CNPJ);
            if (existingCnpj != null)
            {
                throw new InvalidOperationException("Já existe um entregador cadastrado com esse CNPJ.");
            }

            var existingCnh = await _entregadorRepository.GetByNumeroCnhAsync(entregador.NumeroCNH);
            if (existingCnh != null)
            {
                throw new InvalidOperationException("Já existe um entregador cadastrado com esse número de CNH.");
            }

            entregador.FotoCNHUrl = await UploadCnhImageAsync(entregador.CNHImage.Data);

            await _entregadorRepository.AddAsync(entregador);
        }

        public async Task UpdateEntregadorAsync(int id, Entregador entregador)
        {
            var existingEntregador = await _entregadorRepository.GetByIdAsync(id);
            if (existingEntregador == null)
            {
                throw new KeyNotFoundException($"Entregador com o ID {id} não encontrado.");
            }

            ValidateEntregador(entregador);

            if (entregador.CNHImage != null)
            {
                entregador.FotoCNHUrl = await UploadCnhImageAsync(entregador.CNHImage.Data);
            }

            await _entregadorRepository.UpdateAsync(entregador);
        }

        public async Task RemoveEntregadorAsync(int id)
        {
            var existingEntregador = await _entregadorRepository.GetByIdAsync(id);
            if (existingEntregador == null)
            {
                throw new KeyNotFoundException($"Entregador com o ID {id} não encontrado.");
            }

            // Verificar se o entregador tem associações com locações ou pedidos antes de remover
            // Implemente essa lógica de acordo com sua aplicação

            await _entregadorRepository.RemoveAsync(id);
        }

        private async Task<string> UploadCnhImageAsync(byte[] imageData)
        {
            // Verificar se os dados da imagem não são nulos ou vazios
            if (imageData == null || imageData.Length == 0)
            {
                throw new ArgumentNullException(nameof(imageData), "Os dados da imagem da CNH não podem ser nulos ou vazios.");
            }

            // Validar o formato da imagem (png ou bmp)
            if (!IsValidImageFormat(imageData))
            {
                throw new ArgumentException("O formato da imagem da CNH é inválido. O formato deve ser PNG ou BMP.", nameof(imageData));
            }

            try
            {
                // Fazer upload da imagem para o serviço de armazenamento (por exemplo, Amazon S3)
                return await _firebaseStorageService.UploadImageAsync(imageData);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao fazer upload da imagem da CNH.", ex);
            }
        }

        #region Validacao

        private bool IsValidImageFormat(byte[] imageData)
        {
            // Verificar se os primeiros bytes indicam um formato de imagem PNG ou BMP
            return imageData.Length > 10 &&
                   (imageData[1] == 0x50 && imageData[2] == 0x4E && imageData[3] == 0x47) || // PNG
                   (imageData[0] == 0x42 && imageData[1] == 0x4D); // BMP
        }

        private void ValidateEntregador(Entregador entregador)
        {
            if (entregador == null)
            {
                throw new ArgumentNullException(nameof(entregador), "O entregador não pode ser nulo.");
            }

            if (string.IsNullOrWhiteSpace(entregador.Nome))
            {
                throw new ArgumentException("O nome do entregador é obrigatório.", nameof(entregador.Nome));
            }

            if (string.IsNullOrWhiteSpace(entregador.CNPJ))
            {
                throw new ArgumentException("O CNPJ do entregador é obrigatório.", nameof(entregador.CNPJ));
            }

            if (string.IsNullOrWhiteSpace(entregador.NumeroCNH))
            {
                throw new ArgumentException("O número da CNH do entregador é obrigatório.", nameof(entregador.NumeroCNH));
            }

            if (entregador.DataNascimento == default || entregador.DataNascimento >= DateTime.Now)
            {
                throw new ArgumentException("A data de nascimento do entregador é inválida.", nameof(entregador.DataNascimento));
            }

            if (entregador.TipoCNH != TipoCNH.A && entregador.TipoCNH != TipoCNH.B && entregador.TipoCNH != TipoCNH.AB)
            {
                throw new ArgumentException("O tipo da CNH do entregador é inválido.", nameof(entregador.TipoCNH));
            }

            // Validar se o CNPJ é único
            // Adicionar lógica para verificar se o CNPJ já existe em outro entregador

            // Validar se o número da CNH é único
            // Adicionar lógica para verificar se o número da CNH já existe em outro entregador
        }

        #endregion
    }
}
