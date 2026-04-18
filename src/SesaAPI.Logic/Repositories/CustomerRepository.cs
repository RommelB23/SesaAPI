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
    public interface ICustomerRepository
    {
        (bool Success, Customer Model, string Msg) CreateCustomer(CreateCustomerDto m);
        (bool Success, string Msg) UpdateCustomer(UpdateCustomerDto m);
        (bool Success, string Msg) DeleteCustomer(int id);
        IEnumerable<Customer> GetCustomers();
        Customer? GetCustomerById(int id);
        void SaveChanges();
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly SesaDBContext _context;

        public CustomerRepository(SesaDBContext context)
        {
            _context = context;
        }

        public (bool Success, Customer Model, string Msg) CreateCustomer(CreateCustomerDto m)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.Identification == m.Identification);

            if (customer != null)
                return (false, customer, "Ya se ha registrado un cliente con esta identificación");

            customer = new Customer
            {
                FullName = m.FullName,
                Identification = m.Identification,
                Email = m.Email,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            _context.Customers.Add(customer);

            return (true, customer, String.Empty);
        }

        public (bool Success, string Msg) UpdateCustomer(UpdateCustomerDto m)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.Id == m.Id && x.IsActive);
            if (customer == null)
                return (false, "El cliente no existe");

            customer.FullName = !String.IsNullOrEmpty(m.FullName) ? m.FullName : customer.FullName;
            customer.Identification = !String.IsNullOrEmpty(m.Identification) ? m.Identification : customer.Identification;
            customer.Email = !String.IsNullOrEmpty(m.Email) ? m.Email : customer.Email;

            return (true, "Cliente actualizado exitosamente");
        }

        public (bool Success, string Msg) DeleteCustomer(int id)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.Id == id && x.IsActive);
            if (customer == null)
                return (false, "El cliente no existe");

            customer.IsActive = false;

            return (true, "Cliente eliminado exitosamente");
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers.Where(x => x.IsActive).ToArray();
        }

        public Customer? GetCustomerById(int id)
        {
            return _context.Customers.FirstOrDefault(x => x.Id == id && x.IsActive);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }

    public static class CustomerRepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomerRepository(this IServiceCollection services)
        {
            services.TryAddTransient<ICustomerRepository, CustomerRepository>();
            return services;
        }
    }
}