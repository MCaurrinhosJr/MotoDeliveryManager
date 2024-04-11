using MotoDeliveryManager.Domain.Models.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MotoDeliveryManager.Domain.Models
{
    [Table("Locacao")]
    public class Locacao
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data de término prevista é obrigatória.")]
        public DateTime DataTerminoPrevista { get; set; }

        public DateTime? DataTerminoReal { get; set; }

        [Required(ErrorMessage = "O valor total precisto é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O valor total precisto deve ser maior ou igual a zero.")]
        public decimal ValorTotalPrevisto { get; set; }

        public decimal? ValorTotal { get; set; }

        [Required(ErrorMessage = "O entregador é obrigatório.")]
        public int EntregadorId { get; set; }

        public Entregador Entregador { get; set; }

        [Required(ErrorMessage = "A moto é obrigatória.")]
        public int MotoId { get; set; }

        public Moto Moto { get; set; }

        [Required(ErrorMessage = "O status da locação é obrigatório.")]
        public StatusLocacao Status { get; set; }
    }

    public class AluguelRequest
    {
        // Propriedades para o pedido de aluguel
        public DateTime DataInicio { get; set; }
        public DateTime DataTerminoPrevista { get; set; }
        public int EntregadorId { get; set; }
        public int MotoId { get; set; }
    }

    public class DevolucaoRequest
    {
        // Propriedades para o pedido de devolução
        public int LocacaoId { get; set; }
        public DateTime DataDevolucao { get; set; }
    }

}