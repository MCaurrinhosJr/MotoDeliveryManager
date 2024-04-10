using MotoDeliveryManager.Domain.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoDeliveryManager.Domain.Models
{
    [Table("Pedido")]
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "A data de criação é obrigatória.")]
        public DateTime DataCriacao { get; set; }

        [Required(ErrorMessage = "O valor da corrida é obrigatório.")]
        public decimal ValorCorrida { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        public string Endereco { get; set; }

        public StatusPedido StatusPedido { get; set; }
        
        public int? EntregadorId { get; set; }
        
        public Entregador Entregador { get; set; }
    }
}