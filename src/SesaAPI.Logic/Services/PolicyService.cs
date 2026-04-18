using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SesaAPI.Logic.Services
{
    public interface IPolicyService
    {
        String GeneratePolicyNumber();
    }
    internal class PolicyService : IPolicyService
    {
        public String GeneratePolicyNumber()
        {
            return $"POL-{DateTime.Now:yyyyMMddHHmmss}";
        }
    }

    public static class PolicyServiceCollectionExtensions
    {
        public static IServiceCollection AddPolicyService(this IServiceCollection services)
        {
            services.AddTransient<IPolicyService, PolicyService>();
            return services;
        }
    }
}
