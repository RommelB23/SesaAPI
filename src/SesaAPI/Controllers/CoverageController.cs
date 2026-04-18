using Microsoft.AspNetCore.Mvc;
using SesaAPI.Logic.Dtos;
using SesaAPI.Logic.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace SesaAPI.Controllers
{
    [ApiController]
    [Route("api/coverage")]
    public class CoverageController : ControllerBase
    {
        private readonly ICoverageRepository _coverageRepository;

        public CoverageController(ICoverageRepository coverageRepository)
        {
            _coverageRepository = coverageRepository;
        }

        [SwaggerOperation(
            Summary = "Registra un nuevo tipo de cobertura",
            Description = "Requiere los datos del tipo de cobertura. El nombre de la cobertura no puede estar duplicado en el sistema.",
            OperationId = "CreateCoverage"
        )]
        [SwaggerResponse(201, "Tipo de cobertura registrado exitosamente")]
        [SwaggerResponse(400, "No se pudo registrar el tipo de cobertura")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("create")]
        public IActionResult CreateCoverage(CreateCoverageDto m)
        {
            var result = _coverageRepository.CreateCoverage(m);
            if (!result.Success)
                return BadRequest(new { result.Msg });

            _coverageRepository.SaveChanges();

            return CreatedAtAction(nameof(GetCoverageById), new { id = result.Model.Id }, m);
        }

        [SwaggerOperation(
            Summary = "Actualiza un tipo de cobertura existente",
            Description = "Requiere los datos del tipo de cobertura",
            OperationId = "UpdateCoverage"
        )]
        [SwaggerResponse(200, "Tipo de cobertura actualizado exitosamente")]
        [SwaggerResponse(400, "Ocurrió un error actualizando el tipo de cobertura")]
        [SwaggerResponse(404, "No se pudo encontrar el tipo de cobertura")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPut("update")]
        public IActionResult UpdateCoverage(UpdateCoverageDto m)
        {
            var result = _coverageRepository.UpdateCoverage(m);
            if (!result.Success)
                return NotFound(new { result.Msg });

            _coverageRepository.SaveChanges();

            return Ok(new { result.Msg });
        }

        [SwaggerOperation(
            Summary = "Obtiene un tipo de cobertura por su Id",
            Description = "Requiere el Id del tipo de cobertura",
            OperationId = "GetCoverageById"
        )]
        [SwaggerResponse(200, "Tipo de cobertura obtenido exitosamente")]
        [SwaggerResponse(400, "Ocurrió un error obteniendo el tipo de cobertura")]
        [SwaggerResponse(404, "No se pudo encontrar el tipo de cobertura")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet("get/{id}")]
        public IActionResult GetCoverageById(int id)
        {
            var result = _coverageRepository.GetCoverageById(id);

            if (result == null)
            {
                return NotFound(new { Msg = "El tipo de cobertura no existe" });
            }

            return Ok(result);
        }

        [SwaggerOperation(
            Summary = "Obtiene todos los tipos de coberturas",
            Description = "Obtiene un listado de todos los tipos de coberturas",
            OperationId = "GetCoverages"
        )]
        [SwaggerResponse(200, "Tipos de coberturas obtenidos exitosamente")]
        [SwaggerResponse(400, "Ocurrió un error buscando los tipos de coberturas")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet("get")]
        public IActionResult GetCoverages()
        {
            var result = _coverageRepository.GetCoverages();

            return Ok(result);
        }

        [SwaggerOperation(
            Summary = "Elimina un tipo de cobertura",
            Description = "Elimina un tipo de cobertura existente (Se hace borrado lógico cambiando a 0 el valor de la columna IsActive)",
            OperationId = "DeleteCoverage"
        )]
        [SwaggerResponse(200, "Tipo de cobertura eliminado exitosamente")]
        [SwaggerResponse(400, "Ocurrió un error eliminando el tipo de cobertura")]
        [SwaggerResponse(404, "No se encontró el tipo de cobertura")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPut("delete")]
        public IActionResult DeleteCoverage(int id)
        {
            var result = _coverageRepository.DeleteCoverage(id);

            if (!result.Success)
            {
                return NotFound(new { result.Msg });
            }

            _coverageRepository.SaveChanges();
            return Ok(new { result.Msg });
        }
    }
}
