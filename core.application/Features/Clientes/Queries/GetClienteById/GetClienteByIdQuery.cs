using core.application.DTOs.Cliente;
using core.application.Interfaces;
using core.application.Wrappers;
using core.domain.Entities;
using MapsterMapper;
using MediatR;

namespace core.application.Features.Clientes.Queries.GetClienteById
{
    public class GetClienteByIdQuery : IRequest<Response<ClienteDto>>
    {
        public int Id { get; set; }

        //
        // Handler
        //

        public class GetClienteByIdQueryHandler : IRequestHandler<GetClienteByIdQuery, Response<ClienteDto>>
        {
            private readonly IRepositoryAsync<Cliente> _repositoryAsync;
            private readonly IMapper _mapper;

            public GetClienteByIdQueryHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper)
            {
                _repositoryAsync = repositoryAsync;
                _mapper = mapper;
            }

            public async Task<Response<ClienteDto>> Handle(GetClienteByIdQuery request, CancellationToken cancellationToken)
            {
                var cliente = await _repositoryAsync.GetByIdAsync(request.Id);

                if (cliente == null)
                {
                    throw new KeyNotFoundException($"Cliente con Id {request.Id} no encontrado");
                }
                else
                {
                    var dto = _mapper.Map<ClienteDto>(cliente);

                    return new Response<ClienteDto>(dto);
                }
            }
        }
    }
}
