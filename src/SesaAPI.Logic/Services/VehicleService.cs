using Microsoft.Extensions.DependencyInjection;
using SesaAPI.Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SesaAPI.Logic.Services
{
    public interface IVehicleService
    {
        bool IsValidPlate(String plate);
    }
    internal class VehicleService : IVehicleService
    {
        public bool IsValidPlate(String plate)
        {
            return Regex.IsMatch(plate.ToUpper().Trim(), @"^[A-Z]{1,2}[0-9]{6}$");
        }
    }

    public static class VehicleServiceCollectionExtensions
    {
        public static IServiceCollection AddVehicleService(this IServiceCollection services)
        {
            services.AddTransient<IVehicleService, VehicleService>();
            return services;
        }
    }
}
