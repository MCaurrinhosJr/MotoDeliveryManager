using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Interfaces.Services;
using MotoDeliveryManager.Domain.Models;
using MotoDeliveryManager.Domain.Models.Enum;
using MotoDeliveryManager.Domain.Services.FirebaseStorage;
using System.Text.RegularExpressions;

namespace MotoDeliveryManager.Domain.Services
{
    public class EntregadorService : IEntregadorService
    {
        private readonly IEntregadorRepository _entregadorRepository;
        private readonly ILocacaoService _locacaoService;
        private readonly IPedidoService _pedidoService;
        private readonly IFirebaseStorageService _firebaseStorageService;

        public EntregadorService(IEntregadorRepository entregadorRepository, IFirebaseStorageService firebaseStorageService, ILocacaoService locacaoService, IPedidoService pedidoService)
        {
            _entregadorRepository = entregadorRepository;
            _firebaseStorageService = firebaseStorageService;
            _locacaoService = locacaoService;
            _pedidoService = pedidoService;
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

            if (!ValidaCnpj(entregador.CNPJ))
            {
                throw new InvalidOperationException("O numero do CNPJ informado é invalido.");
            }

            if (!ValidaCNH(entregador.NumeroCNH))
            {
                throw new InvalidOperationException("O numero da CNH informado é invalido.");
            }

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
                throw new InvalidOperationException($"Entregador com o ID {id} não encontrado.");
            }

            // Verificar se o entregador tem associações com locações ou pedidos antes de remover
            var existingLocacao = await _locacaoService.GetLocacoesByEntregadorIdAsync(id);
            if (existingLocacao.Any())
            {
                throw new InvalidOperationException($"Não é possível remover este entregador porque ele está associado a uma ou mais locações.");
            }

            var existingPedidos = await _pedidoService.GetPedidosByEntregadorIdAsync(id);
            if (existingPedidos.Any())
            {
                throw new InvalidOperationException("Não é possível remover este entregador porque ele está associado a um ou mais pedidos.");
            }

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
        }

        private bool ValidaCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }

        private bool ValidaCNH(string Cnh)
        {
            char primeiroDigito = Cnh[0];

            if (Regex.Replace(Cnh, @"\D+", "").Length != 11 || string.Format("{0:D11}", 0).Replace('0', primeiroDigito) == Cnh)
            {
                return false;
            }

            long soma = 0, peso = 9;

            for (int i = 0; i < 9; ++i, --peso)
            {
                soma += ((Cnh[i] - '0') * peso);
            }

            long desconto = 0, primeiroDV = soma % 11;

            if (primeiroDV >= 10)
            {
                primeiroDV = 0;
                desconto = 2;
            }

            soma = 0;
            peso = 1;

            for (int i = 0; i < 9; ++i, ++peso)
            {
                soma += ((Cnh[i] - '0') * peso);
            }

            long resto = soma % 11;
            long segundoDV = (resto >= 10) ? 0 : resto - desconto;

            string digitosVerificadores = primeiroDV.ToString() + segundoDV.ToString();

            return digitosVerificadores == Cnh.Substring(Cnh.Length - 2);
        }

        #endregion
    }
}
