using core.application.Exceptions;
using core.application.Interfaces;
using core.application.Wrappers;
using core.domain.Entities;
using MapsterMapper;
using MediatR;

namespace core.application.Features.Clientes.Commands.DeleteClienteCommand
{
    public class DeleteClienteCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }

    //
    // Handler Class
    //

    public class DeleteClienteCommandHandler : IRequestHandler<DeleteClienteCommand, Response<int>>
    {
        private readonly IRepositoryAsync<Cliente> _repositoryAsync;
        private readonly IMapper _mapper;

        public DeleteClienteCommandHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(DeleteClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = _repositoryAsync.GetByIdAsync(request.Id).Result;

            if(cliente == null)
            {
                throw new KeyNotFoundException($"Cliente con Id {request.Id} no encontrado");
            }
            else
            {
                await _repositoryAsync.DeleteAsync(cliente);

                return new Response<int>(cliente.Id);
            }
        }
    }
}
