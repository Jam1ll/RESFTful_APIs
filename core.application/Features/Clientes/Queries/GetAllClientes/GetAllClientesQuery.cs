using core.application.DTOs.Cliente;
using core.application.Interfaces;
using core.application.Specifications;
using core.application.Wrappers;
using core.domain.Entities;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace core.application.Features.Clientes.Queries.GetAllClientes
{
    public class GetAllClientesQuery : IRequest<PagedResponse<List<ClienteDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public class GetAllClientesQueryHandler : IRequestHandler<GetAllClientesQuery, PagedResponse<List<ClienteDto>>>
        {
            private readonly IRepositoryAsync<Cliente> _repositoryAsync;
            private readonly IMapper _mapper;
            private readonly IDistributedCache _distributedCache;

            public GetAllClientesQueryHandler(IRepositoryAsync<Cliente> repositoryAsync, IMapper mapper, IDistributedCache distributedCache)
            {
                _repositoryAsync = repositoryAsync;
                _mapper = mapper;
                _distributedCache = distributedCache;
            }

            public async Task<PagedResponse<List<ClienteDto>>> Handle(GetAllClientesQuery request, CancellationToken cancellationToken)
            {
                //redis cache

                //se crea una key para cada consulta posible
                var cacheKey = $"listadoClientes_{request.PageSize}_{request.PageNumber}_{request.Nombre}_{request.Apellido}";
                string serializedListadoClientes;

                var redisListadoClientes = await _distributedCache.GetAsync(cacheKey);
                var listadoClientes = new List<Cliente>();

                if (redisListadoClientes != null)
                {
                    //si el listado ya existe, se deserializa y decodifica (para redis),
                    //luego se mapea en clientesDto sin llamar a la BD
                    serializedListadoClientes = Encoding.UTF8.GetString(redisListadoClientes);
                    listadoClientes = JsonConvert.DeserializeObject<List<Cliente>>(serializedListadoClientes);
                }
                else
                {
                    //si el listado no existe, se hace una consulta a la BD,
                    //luego se serializa y codifica (para redis) para futuros usos
                    listadoClientes = await _repositoryAsync.ListAsync(new PagedClientesSpecification(request.PageSize, request.PageNumber, request.Nombre, request.Apellido));
                    serializedListadoClientes = JsonConvert.SerializeObject(listadoClientes);
                    redisListadoClientes = Encoding.UTF8.GetBytes(serializedListadoClientes);

                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)) //tiempo de duración de la cache mientras se solicita
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2)); //tiempo de espera hasta que se solicite (sino, se borra)

                    await _distributedCache.SetAsync(cacheKey, redisListadoClientes, options);
                }

                var clientesDto = _mapper.Map<List<ClienteDto>>(listadoClientes ?? []);

                return new PagedResponse<List<ClienteDto>>(clientesDto, request.PageNumber, request.PageSize);
            }
        }
    }
}
