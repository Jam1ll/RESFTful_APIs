using core.application.Parameters;

namespace core.application.Features.Clientes.Queries.GetAllClientes
{
    public class GetAllClientesParameters : RequestParameter
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
    }
}
