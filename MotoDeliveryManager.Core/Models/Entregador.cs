using MotoDeliveryManager.Domain.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoDeliveryManager.Domain.Models
{
    [Table("Entregador")]
    public class Entregador
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        public string CNPJ { get; set; }

        [Required(ErrorMessage = "O número da CNH é obrigatório.")]
        public string NumeroCNH { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O tipo de CNH é obrigatório.")]
        public TipoCNH TipoCNH { get; set; }
        public string FotoCNHUrl { get; set; }
        public CNHImage CNHImage { get; set; }
        public List<Pedido> Pedidos { get; set; }
    }
}