using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Interfaces.Services;
using MotoDeliveryManager.Domain.Models;
using MotoDeliveryManager.Domain.Models.Enum;

namespace MotoDeliveryManager.Domain.Services
{
    public class LocacaoService : ILocacaoService
    {
        private readonly ILocacaoRepository _locacaoRepository;

        public LocacaoService(ILocacaoRepository locacaoRepository)
        {
            _locacaoRepository = locacaoRepository;
        }

        public async Task<List<Locacao>> GetAllLocacoesAsync()
        {
            return await _locacaoRepository.GetAllAsync();
        }

        public async Task<Locacao> GetLocacaoByIdAsync(int id)
        {
            return await _locacaoRepository.GetByIdAsync(id);
        }

        public async Task<Locacao> AlugarMotoAsync(AluguelRequest request)
        {
            if (request.DataTerminoPrevista.Date <= request.DataInicio.Date)
            {
                throw new ArgumentException("A data de término deve ser posterior à data de início.");
            }

            // Calcular o número de dias da locação
            var diasLocacao = (int)(request.DataTerminoPrevista.Date - request.DataInicio.Date).TotalDays;

            // Calcular o valor total da locação com base nos planos disponíveis
            decimal valorTotal;
            switch (diasLocacao)
            {
                case 7:
                    valorTotal = diasLocacao * 30; // Plano de 7 dias
                    break;
                case 15:
                    valorTotal = diasLocacao * 28; // Plano de 15 dias
                    break;
                case 30:
                    valorTotal = diasLocacao * 22; // Plano de 30 dias
                    break;
                default:
                    throw new ArgumentException("A duração da locação não corresponde a nenhum dos planos disponíveis.");
            }

            var locacao = new Locacao
            {
                DataInicio = request.DataInicio,
                DataTerminoPrevista = request.DataTerminoPrevista,
                ValorTotalPrevisto = valorTotal,
                EntregadorId = request.EntregadorId,
                MotoId = request.MotoId,
                Status = StatusLocacao.Ativa
            };

            await _locacaoRepository.AddAsync(locacao);
            return locacao;
        }

        public async Task DevolverMotoAsync(DevolucaoRequest request)
        {
            var locacao = await _locacaoRepository.GetByIdAsync(request.LocacaoId);
            if (locacao == null)
            {
                throw new ArgumentException("Locação não encontrada.");
            }

            locacao.ValorTotal = locacao.ValorTotalPrevisto;

            // Verificar se a data de devolução é anterior à data prevista de término da locação
            if (request.DataDevolucao.Date < locacao.DataTerminoPrevista.Date)
            {
                // Calcular a multa conforme o plano escolhido
                decimal multa;
                switch ((int)(locacao.DataTerminoPrevista.Date - locacao.DataInicio.Date).TotalDays)
                {
                    case 7:
                        multa = locacao.ValorTotalPrevisto * 0.2m; // 20% do valor total
                        break;
                    case 15:
                        multa = locacao.ValorTotalPrevisto * 0.4m; // 40% do valor total
                        break;
                    case 30:
                        multa = locacao.ValorTotalPrevisto * 0.6m; // 60% do valor total
                        break;
                    default:
                        throw new ArgumentException("A duração da locação não corresponde a nenhum dos planos disponíveis.");
                }

                // Cobrar a multa do entregador
                locacao.ValorTotal += multa;
            }
            else if (request.DataDevolucao.Date > locacao.DataTerminoPrevista.Date)
            {
                // Verificar se a duração da locação corresponde a um dos planos disponíveis
                int diasLocacao = (int)(locacao.DataTerminoPrevista.Date - locacao.DataInicio.Date).TotalDays;
                switch (diasLocacao)
                {
                    case 7:
                    case 15:
                    case 30:
                        // Calcular o número de dias extras
                        var diasExtras = (int)(request.DataDevolucao.Date - locacao.DataTerminoPrevista.Date).TotalDays;

                        // Calcular o valor adicional por diária extra
                        var valorDiariaExtra = 50; // Valor adicional por diária extra

                        // Cobrar o valor adicional por diária extra do entregador
                        locacao.ValorTotal += diasExtras * valorDiariaExtra;
                        break;
                    default:
                        throw new ArgumentException("A duração da locação não corresponde a nenhum dos planos disponíveis.");
                }
            }

            // Atualizar a data de término real e o status da locação
            locacao.DataTerminoReal = request.DataDevolucao;
            locacao.Status = StatusLocacao.Concluida;

            await _locacaoRepository.UpdateAsync(locacao);
        }

    }
}