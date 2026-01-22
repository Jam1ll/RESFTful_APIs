using core.application.Exceptions;
using core.application.Interfaces;
using core.application.Wrappers;
using core.domain.Entities;
using MapsterMapper;
using MediatR;

namespace core.application.Features.Clientes.Commands.UpdateClienteCommand
{
    public class UpdateClienteCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
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

    public class UpdateClientCommandHandler : IRequestHandler<UpdateClienteCommand, Response<int>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;
        private readonly IMapper _mapper;

        public UpdateClientCommandHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = await _repositoryAsync.GetByIdAsync(request.Id);

            if(cliente == null)
            {
                throw new KeyNotFoundException($"Registro no encontrado con el id '{request.Id}");
            }
            else
            {
                cliente.Nombre = request.Nombre;
                cliente.Apellido = request.Apellido;
                cliente.FechaNacimiento = request.FechaNacimiento;
                cliente.Telefono = request.Telefono;
                cliente.Email = request.Email;
                cliente.Direccion = request.Direccion;

                await _repositoryAsync.UpdateAsync(cliente);

                return new Response<int>(cliente.Id);
            }
        }
    }
}
