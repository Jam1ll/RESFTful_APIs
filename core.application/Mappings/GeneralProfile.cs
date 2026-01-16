using AutoMapper;
using core.application.Features.Clientes.Commands.CreateClienteCommand;
using core.domain.Entities;

namespace core.application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region commands
            CreateMap<CreateClienteCommand, Cliente>();
            #endregion
        }
    }
}
