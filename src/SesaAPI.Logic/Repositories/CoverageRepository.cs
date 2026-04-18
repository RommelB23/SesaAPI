using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SesaAPI.Data.Context;
using SesaAPI.Data.Models;
using SesaAPI.Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SesaAPI.Logic.Repositories
{
    public interface ICoverageRepository
    {
        (bool Success, Coverage Model, string Msg) CreateCoverage(CreateCoverageDto m);
        (bool Success, string Msg) UpdateCoverage(UpdateCoverageDto m);
        (bool Success, string Msg) DeleteCoverage(int id);
        IEnumerable<Coverage> GetCoverages();
        Coverage? GetCoverageById(int id);
        void SaveChanges();
    }

    public class CoverageRepository : ICoverageRepository
    {
        private readonly SesaDBContext _context;

        public CoverageRepository(SesaDBContext context)
        {
            _context = context;
        }

        public (bool Success, Coverage Model, string Msg) CreateCoverage(CreateCoverageDto m)
        {
            var coverage = _context.Coverages.FirstOrDefault(x => x.Name == m.Name);

            if (coverage != null)
                return (false, coverage, "Ya se ha registrado un tipo de cobertura con esta descripción");

            coverage = new Coverage
            {
                Name = m.Name,
                Rate = m.Rate,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            _context.Coverages.Add(coverage);

            return (true, coverage, String.Empty);
        }

        public (bool Success, string Msg) UpdateCoverage(UpdateCoverageDto m)
        {
            var coverage = _context.Coverages.FirstOrDefault(x => x.Id == m.Id && x.IsActive);
            if (coverage == null)
                return (false, "El tipo de cobertura no existe");

            coverage.Name = !String.IsNullOrEmpty(m.Name) ? m.Name : coverage.Name;
            coverage.Rate = m.Rate != 0 ? m.Rate : coverage.Rate;

            return (true, "El tipo de cobertura ha sido actualizado exitosamente");
        }

        public (bool Success, string Msg) DeleteCoverage(int id)
        {
            var coverage = _context.Coverages.FirstOrDefault(x => x.Id == id && x.IsActive);
            if (coverage == null)
                return (false, "El tipo de cobertura no existe");

            coverage.IsActive = false;

            return (true, "Tipo de cobertura eliminada exitosamente");
        }

        public IEnumerable<Coverage> GetCoverages()
        {
            return _context.Coverages.Where(x => x.IsActive).ToArray();
        }

        public Coverage? GetCoverageById(int id)
        {
            return _context.Coverages.FirstOrDefault(x => x.Id == id && x.IsActive);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }

    public static class CoverageRepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddCoverageRepository(this IServiceCollection services)
        {
            services.TryAddTransient<ICoverageRepository, CoverageRepository>();
            return services;
        }

    }
}
