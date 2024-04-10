using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoDeliveryManager.Domain.Models
{
    [Table("Moto")]
    public class Moto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "A placa é obrigatória.")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "A marca é obrigatória.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "O modelo é obrigatório.")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "O Ano é obrigatório.")]
        public string Ano {  get; set; }
        public List<Locacao> Locacoes { get; set; }
    }
}