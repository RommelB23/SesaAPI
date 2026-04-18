using Microsoft.AspNetCore.Mvc;
using SesaAPI.Logic.Dtos;
using SesaAPI.Logic.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace SesaAPI.Controllers
{
    [ApiController]
    [Route("api/policy")]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyRepository _policyRepository;

        public PolicyController(IPolicyRepository policyRepository)
        {
            _policyRepository = policyRepository;
        }

        [SwaggerOperation(
            Summary = "Emite una póliza de seguro",
            Description = "Crea una nueva póliza de seguro. Si se envía el Id de vehículo con valor 0 y una placa sin registrar, se toma como un registro nuevo y se guarda. " +
            "Se valida que un cliente no tenga dos pólizas activas para la misma placa y que no se pueda emitir si el vehículo tiene más de 20 años de antigüedad",
            OperationId = "CreatePolicy"
        )]
        [SwaggerResponse(201, "Póliza emitida exitosamente")]
        [SwaggerResponse(400, "No se pudo emitir la póliza")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("create")]
        public IActionResult CreatePolicy(CreatePolicyDto m)
        {
            var result = _policyRepository.CreatePolicy(m);
            if (!result.Success)
                return BadRequest(new { result.Msg });

            _policyRepository.SaveChanges();
            return CreatedAtAction(nameof(GetPolicyById), new { id = result.Model?.Id }, m);
        }

        [SwaggerOperation(
            Summary = "Actualiza una póliza de seguro",
            Description = "Actualiza una póliza de seguro existente (Solo los tipos de cobertura)",
            OperationId = "UpdatePolicy"
        )]
        [SwaggerResponse(200, "Póliza actualizada exitosamente")]
        [SwaggerResponse(400, "Ocurrió un error actualizando la póliza")]
        [SwaggerResponse(404, "No se pudo encontrar la póliza")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPut("update")]
        public IActionResult UpdatePolicy(UpdatePolicyDto m)
        {
            var result = _policyRepository.UpdatePolicy(m);
            if (!result.Success)
                return NotFound(new { result.Msg });

            _policyRepository.SaveChanges();
            return Ok(new { result.Msg });
        }

        [SwaggerOperation(
            Summary = "Obtiene una póliza por su Id",
            Description = "Requiere el Id de la póliza",
            OperationId = "GetPolicyById"
        )]
        [SwaggerResponse(200, "Póliza obtenida exitosamente")]
        [SwaggerResponse(400, "Ocurrió un error obteniendo la póliza")]
        [SwaggerResponse(404, "No se pudo encontrar la póliza")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet("get/{id}")]
        public IActionResult GetPolicyById(int id)
        {
            var result = _policyRepository.GetPolicyById(id);

            if (result == null)
            {
                return NotFound(new { Msg = "La póliza no existe" });
            }

            return Ok(result);
        }

        [SwaggerOperation(
            Summary = "Obtiene todas las pólizas emitidas",
            Description = "Obtiene un listado de todas las pólizas emitidas",
            OperationId = "GetPolicies"
        )]
        [SwaggerResponse(200, "Pólizas obtenidas exitosamente")]
        [SwaggerResponse(400, "Ocurrió un error buscando las pólizas")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet("get")]
        public IActionResult GetPolicies()
        {
            var result = _policyRepository.GetPolicies();

            return Ok(result);
        }

        [SwaggerOperation(
            Summary = "Elimina una póliza",
            Description = "Elimina una póliza existente (Se hace borrado lógico cambiando a 0 el valor de la columna IsActive)",
            OperationId = "DeletePolicy"
        )]
        [SwaggerResponse(200, "Póliza eliminada exitosamente")]
        [SwaggerResponse(400, "Ocurrió un error eliminando la póliza")]
        [SwaggerResponse(404, "No se encontró la póliza")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPut("delete")]
        public IActionResult DeletePolicy(int id)
        {
            var result = _policyRepository.DeletePolicy(id);
            if (!result.Success)
            {
                return NotFound(new { result.Msg });
            }

            _policyRepository.SaveChanges();
            return Ok(new { result.Msg });
        }
    }
}
