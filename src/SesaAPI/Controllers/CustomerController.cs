using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using SesaAPI.Logic.Dtos;
using SesaAPI.Logic.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace SesaAPI.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [SwaggerOperation(
            Summary = "Registra un nuevo cliente",
            Description = "Requiere los datos del cliente. La identificación no puede estar duplicada en el sistema.",
            OperationId = "CreateCustomer"
        )]
        [SwaggerResponse(201, "Cliente registrado exitosamente")]
        [SwaggerResponse(400, "No se pudo registrar el cliente")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("create")]
        public IActionResult CreateCustomer(CreateCustomerDto m)
        {
            var result = _customerRepository.CreateCustomer(m);
            if (!result.Success)
                return BadRequest(new { result.Msg });

            _customerRepository.SaveChanges();
            
            return CreatedAtAction(nameof(GetCustomerById), new { id = result.Model.Id }, m);
        }

        [SwaggerOperation(
            Summary = "Actualiza un cliente existente",
            Description = "Requiere los datos del cliente",
            OperationId = "UpdateCustomer"
        )]
        [SwaggerResponse(200, "Cliente actualizado exitosamente")]
        [SwaggerResponse(400, "Ocurrió un error actualizando el cliente")]
        [SwaggerResponse(404, "No se pudo encontrar el cliente")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPut("update")]
        public IActionResult UpdateCustomer(UpdateCustomerDto m)
        {
            var result = _customerRepository.UpdateCustomer(m);
            if (!result.Success)
                return NotFound(new { result.Msg });

            _customerRepository.SaveChanges();

            return Ok(new { result.Msg});
        }

        [SwaggerOperation(
            Summary = "Obtiene un cliente por su Id",
            Description = "Requiere el Id del cliente",
            OperationId = "GetCustomerById"
        )]
        [SwaggerResponse(200, "Cliente obtenido exitosamente")]
        [SwaggerResponse(400, "Ocurrió un error obteniendo el cliente")]
        [SwaggerResponse(404, "No se pudo encontrar el cliente")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet("get/{id}")]
        public IActionResult GetCustomerById(int id)
        {
            var result = _customerRepository.GetCustomerById(id);

            if(result == null)
            {
                return NotFound(new { Msg = "El cliente no existe" });
            }

            return Ok(result);
        }

        [SwaggerOperation(
            Summary = "Obtiene todos los clientes",
            Description = "Obtiene un listado de todos los clientes",
            OperationId = "GetCustomers"
        )]
        [SwaggerResponse(200, "Clientes obtenidos exitosamente")]
        [SwaggerResponse(400, "Ocurrió un error buscando los clientes")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet("get")]
        public IActionResult GetCustomers()
        {
            var result = _customerRepository.GetCustomers();

            return Ok(result);
        }

        [SwaggerOperation(
            Summary = "Elimina un cliente",
            Description = "Elimina un cliente existente (Se hace borrado lógico cambiando a 0 el valor de la columna IsActive)",
            OperationId = "DeleteCustomer"
        )]
        [SwaggerResponse(200, "Cliente eliminado exitosamente")]
        [SwaggerResponse(400, "Ocurrió un error eliminando el cliente")]
        [SwaggerResponse(404, "No se encontró el cliente")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPut("delete")]
        public IActionResult DeleteCustomer(int id)
        {
            var result = _customerRepository.DeleteCustomer(id);

            if (!result.Success)
            {
                return NotFound(new { result.Msg });
            }

            _customerRepository.SaveChanges();
            return Ok(new { result.Msg });
        }
    }
}
