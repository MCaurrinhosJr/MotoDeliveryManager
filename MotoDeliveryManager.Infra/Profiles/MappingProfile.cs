using AutoMapper;
using MotoDeliveryManager.Domain.Models;
using MotoDeliveryManager.Infra.Context;

namespace MotoDeliveryManager.Infra.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Moto, Moto>(); // Mapeamento de Moto para Moto
            CreateMap<Pedido, Pedido>(); // Mapeamento de Pedido para Pedido
            CreateMap<Entregador, Entregador>(); // Mapeamento de Entregador para Entregador
            CreateMap<Locacao, Locacao>(); // Mapeamento de Locacao para Locacao
            CreateMap<Notificacao, Notificacao>(); // Mapeamento de Notificacao para Notificacao
        }
    }
}
