using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoDeliveryManager.Domain.Models
{
    [Table("Notificacao")]
    public class Notificacao
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "A mensagem é obrigatória.")]
        public string Mensagem { get; set; }

        [Required(ErrorMessage = "A data de envio é obrigatória.")]
        public DateTime DataEnvio { get; set; }

        [Required(ErrorMessage = "O entregador é obrigatório.")]
        public int EntregadorId { get; set; }

        public Entregador Entregador { get; set; }

        [Required(ErrorMessage = "O pedido é obrigatório.")]
        public int PedidoId { get; set; }

        public Pedido Pedido { get; set; }
    }
}