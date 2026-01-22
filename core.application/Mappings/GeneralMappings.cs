using core.application.DTOs.Cliente;
using core.application.Features.Clientes.Commands.CreateClienteCommand;
using core.domain.Entities;
using Mapster;

namespace core.application.Mappings
{
    public class GeneralMappings : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            #region commands to entities
            config.NewConfig<CreateClienteCommand, Cliente>();
            #endregion

            #region DTOs
            config.NewConfig<Cliente, ClienteDto>();
            #endregion

            // Si se necesitara un mapeo personalizado, sería así:
            // config.NewConfig<CreateClienteCommand, Cliente>()
            //       .Map(dest => dest.NombreCompleto, src => src.Nombre + " " + src.Apellido);
        }
    }
}
