using Microsoft.AspNetCore.Mvc;
using SesaAPI.Logic.Dtos;
using SesaAPI.Logic.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace SesaAPI.Controllers
{
    [ApiController]
    [Route("api/vehicle")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleController(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        [SwaggerOperation(
            Summary = "Registra un nuevo vehículo",
            Description = "Requiere los datos del vehículo. La placa debe tener un formato de una o dos letras y seis números, y no se permiten placas duplicadas en el sistema.",
            OperationId = "CreateVehicle"
        )]
        [SwaggerResponse(201, "Vehículo registrado exitosamente")]
        [SwaggerResponse(400, "No se pudo registrar el vehículo")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("create")]
        public IActionResult CreateVehicle(CreateVehicleDto m)
        {
            var result = _vehicleRepository.CreateVehicle(m);
            if (!result.Success)
                return BadRequest(new { result.Msg });

            _vehicleRepository.SaveChanges();
            return CreatedAtAction(nameof(GetVehicleById), new { id = result.Model?.Id }, m);
        }

        [SwaggerOperation(
            Summary = "Actualiza un vehículo existente",
            Description = "Requiere los datos del vehículo",
            OperationId = "UpdateVehicle"
        )]
        [SwaggerResponse(200, "Vehículo actualizado exitosamente")]
        [SwaggerResponse(400, "Ocurrió un error actualizando el vehículo")]
        [SwaggerResponse(404, "No se pudo encontrar el vehículo")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPut("update")]
        public IActionResult UpdateVehicle(UpdateVehicleDto m)
        {
            var result = _vehicleRepository.UpdateVehicle(m);
            if (!result.Success)
                return NotFound(new { result.Msg });

            _vehicleRepository.SaveChanges();
            return Ok(new { result.Msg });
        }

        [SwaggerOperation(
            Summary = "Obtiene un vehículo por su Id",
            Description = "Requiere el Id del vehículo",
            OperationId = "GetVehicleById"
        )]
        [SwaggerResponse(200, "Vehículo obtenido exitosamente")]
        [SwaggerResponse(400, "Ocurrió un error obteniendo el vehículo")]
        [SwaggerResponse(404, "No se pudo encontrar el vehículo")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet("get/{id}")]
        public IActionResult GetVehicleById(int id)
        {
            var result = _vehicleRepository.GetVehicleById(id);

            if (result == null)
            {
                return NotFound(new { Msg = "El vehículo no existe" });
            }

            return Ok(result);
        }

        [SwaggerOperation(
            Summary = "Obtiene todos los vehículos",
            Description = "Obtiene un listado de todos los vehículos",
            OperationId = "GetVehicles"
        )]
        [SwaggerResponse(200, "Vehículos obtenidos exitosamente")]
        [SwaggerResponse(400, "Ocurrió un error buscando los vehículos")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet("get")]
        public IActionResult GetVehicles()
        {
            var result = _vehicleRepository.GetVehicles();

            return Ok(result);
        }

        [SwaggerOperation(
            Summary = "Elimina un vehículo",
            Description = "Elimina un vehículo existente (Se hace borrado lógico cambiando a 0 el valor de la columna IsActive)",
            OperationId = "DeleteVehicle"
        )]
        [SwaggerResponse(200, "Vehículo eliminado exitosamente")]
        [SwaggerResponse(400, "Ocurrió un error eliminando el vehículo")]
        [SwaggerResponse(404, "No se encontró el vehículo")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPut("delete")]
        public IActionResult DeleteVehicle(int id)
        {
            var result = _vehicleRepository.DeleteVehicle(id);
            if (!result.Success)
            {
                return NotFound(new { result.Msg });
            }

            _vehicleRepository.SaveChanges();
            return Ok(new { result.Msg });
        }
    }
}
