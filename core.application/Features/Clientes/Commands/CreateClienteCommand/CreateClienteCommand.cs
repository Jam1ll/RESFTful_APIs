using core.application.Interfaces;
using core.application.Wrappers;
using core.domain.Entities;
using MapsterMapper;
using MediatR;

namespace core.application.Features.Clientes.Commands.CreateClienteCommand
{
    //un command es una solicitud que recibe la API
    //para realizar una acción específica, en este caso, crear un nuevo cliente.
    public class CreateClienteCommand : IRequest<Response<int>>
    {
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public required string Telefono { get; set; }
        public required string Email { get; set; }
        public required string Direccion { get; set; }
    }

    //
    // Handler Class
    //

    public class CreateClienteCommandHandler : IRequestHandler<CreateClienteCommand, Response<int>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;
        private readonly IMapper _mapper;

        public CreateClienteCommandHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }

        //Maneja la lógica para crear un nuevo cliente
        public async Task<Response<int>> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
        {
            var newRecord = _mapper.Map<Cliente>(request);
            var data = await _repositoryAsync.AddAsync(newRecord, cancellationToken);

            return new Response<int>(data.Id);
        }
    }
}
