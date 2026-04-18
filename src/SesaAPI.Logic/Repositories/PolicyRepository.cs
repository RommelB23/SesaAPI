using Microsoft.EntityFrameworkCore;
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
    public interface IPolicyRepository
    {
        (bool Success, Policy? Model, string Msg) CreatePolicy(CreatePolicyDto m);
        (bool Success, string Msg) UpdatePolicy(UpdatePolicyDto m);
        (bool Success, string Msg) DeletePolicy(int id);
        IEnumerable<PolicyResponseDto> GetPolicies();
        PolicyResponseDto? GetPolicyById(int id);
        void SaveChanges();
    }

    internal class PolicyRepository : IPolicyRepository
    {
        private readonly SesaDBContext _context;
        private readonly ICustomerRepository _customerRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IPolicyService _policyService;
        private readonly IVehicleService _vehicleService;
        public PolicyRepository(SesaDBContext context, IPolicyService policyService, IVehicleService vehicleService, IVehicleRepository vehicleRepository, ICustomerRepository customerRepository)
        {
            _context = context;
            _policyService = policyService;
            _vehicleService = vehicleService;
            _vehicleRepository = vehicleRepository;
            _customerRepository = customerRepository;
        }

        public (bool Success, Policy? Model, string Msg) CreatePolicy(CreatePolicyDto m)
        {
            /* Se valida que exista el cliente*/
            var customer = _customerRepository.GetCustomerById(m.CustomerId);

            if (customer == null)
            {
                return (false, null, "El cliente no existe");
            }

            /* Se realiza la búsqueda del vehículo. En caso de enviar un Id 0, se hace registro de un vehículo nuevo */
            var vehicle = _vehicleRepository.GetVehicleById(m.VehicleId);

            if (vehicle == null || m.VehicleId == 0)
            {
                var createVehicle = _vehicleRepository.CreateVehicle(new CreateVehicleDto
                {
                    LicensePlate = m.LicensePlate,
                    Brand = m.Brand,
                    Model = m.Model,
                    Year = m.Year,
                    CommercialValue = m.CommercialValue
                });

                if (createVehicle.Success)
                {
                    vehicle = createVehicle.Model;
                    SaveChanges();
                }
                else
                {
                    return (false, null, createVehicle.Msg);
                }
            }

            /* Se valida la antigüedad del vehículo */
            if ((DateTime.UtcNow.Year - vehicle?.Year) > 20)
            {
                return (false, null, "El vehículo es demasiado antiguo para asegurar");
            }

            /* Se valida una póliza activa por placa */
            var activePolicy = _context.Policies.FirstOrDefault(p => p.Vehicle.LicensePlate == vehicle.LicensePlate && p.CustomerId == customer.Id && p.IsActive);
            if (activePolicy != null)
            {
                return (false, null, "El cliente ya tiene una póliza activa para esta placa");
            }

            /* Se obtienen las coberturas */
            var coverages = _context.Coverages.Where(c => m.CoveragesId.Select(id => id.Id).Contains(c.Id)).ToList();

            if (coverages == null || !coverages.Any())
            {
                return (false, null, "Debe seleccionar al menos una cobertura válida");
            }

            /* Se hacen los cálculos */
            var insuredAmount = vehicle?.CommercialValue * 0.9m; // Se asegura el 90% del valor comercial del vehículo
            var totalRate = coverages.Sum(c => c.Rate);
            var totalPremium = insuredAmount * (totalRate / 100);

            /* Se crea la póliza */
            var policy = new Policy
            {
                PolicyNumber = _policyService.GeneratePolicyNumber(),
                CustomerId = customer.Id,
                VehicleId = vehicle.Id,
                IssueDate = DateTime.UtcNow,
                InsuredAmount = insuredAmount ?? 0,
                TotalPremium = totalPremium ?? 0,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _context.Policies.Add(policy);

            SaveChanges();

            /* Se guardan las coberturas seleccionadas*/
            foreach (var coverage in coverages)
            {
                _context.PolicyCoverages.Add(new PolicyCoverage
                {
                    PolicyId = policy.Id,
                    CoverageId = coverage.Id
                });
            }

            return (true, policy, String.Empty);
        }

        public (bool Success, string Msg) UpdatePolicy(UpdatePolicyDto m)
        {
            var policy = _context.Policies.FirstOrDefault(x => x.Id == m.Id && x.IsActive);
            if (policy == null)
                return (false, "La póliza no existe");

            /* Solo se permite actualizar las coberturas de la póliza */
            var coverages = _context.Coverages.Where(c => m.CoveragesId.Select(id => id.Id).Contains(c.Id)).ToList();
            if (coverages == null || !coverages.Any())
            {
                return (false, "Debe seleccionar al menos una cobertura");
            }

            /* Se eliminan las coberturas actuales */
            var currentCoverages = _context.PolicyCoverages.Where(pc => pc.PolicyId == policy.Id).ToList();
            _context.PolicyCoverages.RemoveRange(currentCoverages);

            /* Se agregan las nuevas coberturas seleccionadas*/
            foreach (var coverage in coverages)
            {
                _context.PolicyCoverages.Add(new PolicyCoverage
                {
                    PolicyId = policy.Id,
                    CoverageId = coverage.Id
                });
            }

            /* Se recalculan los valores de la póliza */
            var totalRate = coverages.Sum(c => c.Rate);
            var totalPremium = policy.InsuredAmount * (totalRate / 100);

            /* Se actualizan los valores de la póliza */
            policy.TotalPremium = totalPremium;

            return (true, "Póliza actualizada exitosamente");
        }

        public (bool Success, string Msg) DeletePolicy(int id)
        {
            var policy = _context.Policies.FirstOrDefault(x => x.Id == id && x.IsActive);
            if (policy == null)
                return (false, "La póliza no existe");
            policy.IsActive = false;
            return (true, "Póliza eliminada exitosamente");
        }

        public IEnumerable<PolicyResponseDto> GetPolicies()
        {

            return _context.Policies.OrderByDescending(p => p.IssueDate)
                .Select(p => new PolicyResponseDto
                {
                    Id = p.Id,
                    PolicyNumber = p.PolicyNumber,
                    CustomerName = p.Customer.FullName,
                    VehiclePlate = p.Vehicle.LicensePlate,
                    InsuredAmount = p.InsuredAmount,
                    TotalPremium = p.TotalPremium,
                    IssueDate = p.IssueDate,
                    IsActive = p.IsActive,
                    Coverages = p.PolicyCoverages
                    .Select(pc => new CreateCoverageDto
                    {
                        Name = pc.Coverage.Name,
                        Rate = pc.Coverage.Rate
                    }).ToList()
                }).ToList();
        }

        public PolicyResponseDto? GetPolicyById(int id)
        {
            return _context.Policies.Where(p => p.Id == id && p.IsActive)
                .Select(p => new PolicyResponseDto
                {
                    Id = p.Id,
                    PolicyNumber = p.PolicyNumber,
                    CustomerName = p.Customer.FullName,
                    VehiclePlate = p.Vehicle.LicensePlate,
                    InsuredAmount = p.InsuredAmount,
                    TotalPremium = p.TotalPremium,
                    IssueDate = p.IssueDate,
                    IsActive = p.IsActive,
                    Coverages = p.PolicyCoverages
                    .Select(pc => new CreateCoverageDto
                    {
                        Name = pc.Coverage.Name,
                        Rate = pc.Coverage.Rate
                    }).ToList()
                }).FirstOrDefault();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }

    public static class PolicyRepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddPolicyRepository(this IServiceCollection services)
        {
            services.TryAddTransient<IPolicyRepository, PolicyRepository>();
            return services;
        }
    }
}