using Asp.Versioning;
using core.application.Features.Clientes.Commands.CreateClienteCommand;
using core.application.Features.Clientes.Commands.DeleteClienteCommand;
using core.application.Features.Clientes.Commands.UpdateClienteCommand;
using core.application.Features.Clientes.Queries.GetAllClientes;
using core.application.Features.Clientes.Queries.GetClienteById;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ClienteController : BaseApiController
    {
        //POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(CreateClienteCommand command)
        {
            //Mediator envia el comando al handler para su procesamiento
            //En este caso, el handler asociado a CreateClienteCommand maneja la creación del cliente.
            return Ok(await Mediator.Send(command));
        }

        //PUT api/<controller>/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateClienteCommand command)
        {
            IdValidator(id, command.Id);
            return Ok(await Mediator.Send(command));
        }

        //DELETE api/<controller>/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteClienteCommand { Id = id }));
        }

        //GET api/<controller>/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetClienteByIdQuery { Id = id }));
        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] GetAllClientesParameters filter)
        {
            return Ok(await Mediator.Send(new GetAllClientesQuery
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                Nombre = filter.Nombre,
                Apellido = filter.Apellido
            }));
        }

        //metodo privado para validar la entrada de Ids
        private ActionResult? IdValidator(int? id, int? commandId = null)
        {
            if (id == null)
                return null;
            if (id != commandId)
                return BadRequest();
            return null;
        }
    }
}
