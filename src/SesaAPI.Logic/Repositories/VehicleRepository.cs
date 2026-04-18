using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SesaAPI.Data.Context;
using SesaAPI.Data.Models;
using SesaAPI.Logic.Dtos;
using SesaAPI.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SesaAPI.Logic.Repositories
{
    public interface IVehicleRepository
    {
        (bool Success, Vehicle? Model, string Msg) CreateVehicle(CreateVehicleDto m);
        (bool Success, string Msg) UpdateVehicle(UpdateVehicleDto m);
        (bool Success, string Msg) DeleteVehicle(int id);
        IEnumerable<Vehicle> GetVehicles();
        Vehicle? GetVehicleById(int id);
        void SaveChanges();
    }

    public class VehicleRepository : IVehicleRepository
    {
        private readonly SesaDBContext _context;
        private readonly IVehicleService _vehicleService;

        public VehicleRepository(SesaDBContext context, IVehicleService vehicleService)
        {
            _context = context;
            _vehicleService = vehicleService;
        }

        public (bool Success, Vehicle? Model, string Msg) CreateVehicle(CreateVehicleDto m)
        {
            if (_vehicleService.IsValidPlate(m.LicensePlate))
            {
                var vehicle = _context.Vehicles.FirstOrDefault(x => x.LicensePlate == m.LicensePlate);

                if (vehicle != null)
                    return (false, vehicle, "Ya se ha registrado un vehículo con esta placa");

                vehicle = new Vehicle
                {
                    LicensePlate = m.LicensePlate,
                    Brand = m.Brand,
                    Model = m.Model,
                    Year = m.Year,
                    CommercialValue = m.CommercialValue,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Vehicles.Add(vehicle);

                return (true, vehicle, String.Empty);
            }

            return (false, null, "La placa del vehículo no es válida");
        }

        public (bool Success, string Msg) UpdateVehicle(UpdateVehicleDto m)
        {
            var vehicle = _context.Vehicles.FirstOrDefault(x => x.Id == m.Id && x.IsActive);
            if (vehicle == null)
                return (false, "El vehículo no existe");

            if (_vehicleService.IsValidPlate(m.LicensePlate))
            {
                vehicle.LicensePlate = !String.IsNullOrEmpty(m.LicensePlate) ? m.LicensePlate : vehicle.LicensePlate;
            } else
            {
                return (false, "La placa del vehículo no es válida");
            }
                
            vehicle.Brand = !String.IsNullOrEmpty(m.Brand) ? m.Brand : vehicle.Brand;
            vehicle.Model = !String.IsNullOrEmpty(m.Model) ? m.Model : vehicle.Model;
            vehicle.Year = m.Year != 0 ? m.Year : vehicle.Year;
            vehicle.CommercialValue = m.CommercialValue != 0 ? m.CommercialValue : vehicle.CommercialValue;

            return (true, "Vehículo actualizado exitosamente");
        }

        public (bool Success, string Msg) DeleteVehicle(int id)
        {
            var vehicle = _context.Vehicles.FirstOrDefault(x => x.Id == id && x.IsActive);
            if (vehicle == null)
                return (false, "El vehículo no existe");

            vehicle.IsActive = false;

            return (true, "Vehículo eliminado exitosamente");
        }

        public IEnumerable<Vehicle> GetVehicles()
        {
            return _context.Vehicles.Where(x => x.IsActive).ToArray();
        }

        public Vehicle? GetVehicleById(int id)
        {
            return _context.Vehicles.FirstOrDefault(x => x.Id == id && x.IsActive);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }

    public static class VehicleRepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddVehicleRepository(this IServiceCollection services)
        {
            services.TryAddTransient<IVehicleRepository, VehicleRepository>();
            return services;
        }

    }
}