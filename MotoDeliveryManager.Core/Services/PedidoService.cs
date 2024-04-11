using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Interfaces.Services;
using MotoDeliveryManager.Domain.Models;
using MotoDeliveryManager.Domain.Models.Enum;
using MotoDeliveryManager.Domain.Services.RabbitMq;

namespace MotoDeliveryManager.Domain.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IRabbitMQService _rabbitMQService;

        public PedidoService(IPedidoRepository pedidoRepository, IRabbitMQService rabbitMQService)
        {
            _pedidoRepository = pedidoRepository;
            _rabbitMQService = rabbitMQService;
        }

        public async Task<List<Pedido>> GetAllPedidosAsync()
        {
            return await _pedidoRepository.GetAllAsync();
        }

        public async Task<Pedido> GetPedidoByIdAsync(int id)
        {
            return await _pedidoRepository.GetByIdAsync(id);
        }

        public async Task AddPedidoAsync(Pedido newPedido)
        {
            if (newPedido == null)
                throw new ArgumentNullException(nameof(newPedido), "Pedido não pode ser nulo.");

            if (newPedido.DataCriacao == default)
                throw new ArgumentException("A data de criação do pedido é obrigatória.", nameof(newPedido.DataCriacao));

            if (newPedido.ValorCorrida <= 0)
                throw new ArgumentException("O valor da corrida deve ser maior que zero.", nameof(newPedido.ValorCorrida));

            newPedido.StatusPedido = StatusPedido.Disponivel;
            var pedido = await _pedidoRepository.AddAsync(newPedido);
            _rabbitMQService.EnviarNotificacaoPedidoDisponivel(pedido);
        }

        public async Task UpdatePedidoAsync(int id, Pedido pedido)
        {
            if (pedido == null)
                throw new ArgumentNullException(nameof(pedido), "Pedido não pode ser nulo.");

            if (pedido.DataCriacao == default)
                throw new ArgumentException("A data de criação do pedido é obrigatória.", nameof(pedido.DataCriacao));

            if (pedido.ValorCorrida <= 0)
                throw new ArgumentException("O valor da corrida deve ser maior que zero.", nameof(pedido.ValorCorrida));

            var existingPedido = await _pedidoRepository.GetByIdAsync(id);
            if (existingPedido == null)
                throw new KeyNotFoundException($"Pedido com ID {id} não encontrado.");

            pedido.Id = id;
            await _pedidoRepository.UpdateAsync(pedido);
        }

        public async Task RemovePedidoAsync(int id)
        {
            await _pedidoRepository.RemoveAsync(id);
        }

        public async Task EntregarPedidoAsync(int id, Pedido pedido, int entregadorId)
        {
            if (pedido == null)
                throw new ArgumentNullException(nameof(pedido), "Pedido não pode ser nulo.");

            if (pedido.DataCriacao == default)
                throw new ArgumentException("A data de criação do pedido é obrigatória.", nameof(pedido.DataCriacao));

            if (pedido.ValorCorrida <= 0)
                throw new ArgumentException("O valor da corrida deve ser maior que zero.", nameof(pedido.ValorCorrida));

            var existingPedido = await _pedidoRepository.GetByIdAsync(id);
            if (existingPedido == null)
                throw new KeyNotFoundException($"Pedido com ID {id} não encontrado.");

            if (existingPedido.EntregadorId != entregadorId)
                throw new KeyNotFoundException($"Entregador ID {entregadorId} não é reponsavel pelo Pedido.");

            pedido.Id = id;
            pedido.StatusPedido = StatusPedido.Entregue;
            await _pedidoRepository.UpdateAsync(pedido);
        }

        public async Task AceitarPedidoAsync(int id, Pedido pedido, int entregadorId)
        {
            if (pedido == null)
                throw new ArgumentNullException(nameof(pedido), "Pedido não pode ser nulo.");

            if (pedido.DataCriacao == default)
                throw new ArgumentException("A data de criação do pedido é obrigatória.", nameof(pedido.DataCriacao));

            if (pedido.ValorCorrida <= 0)
                throw new ArgumentException("O valor da corrida deve ser maior que zero.", nameof(pedido.ValorCorrida));

            var existingPedido = await _pedidoRepository.GetByIdAsync(id);
            if (existingPedido == null)
                throw new KeyNotFoundException($"Pedido com ID {id} não encontrado.");

            pedido.StatusPedido = StatusPedido.Aceito;
            pedido.EntregadorId = entregadorId;
            await _pedidoRepository.UpdateAsync(pedido);
        }
    }
}
