using Hubtel.Wallets.Application.Interface;
using Hubtel.Wallets.Application.Repository;
using Hubtel.Wallets.Application.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Hubtel.Wallets.Application;

public static class ApplicationCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        service.AddScoped<ICustomerRepository, CustomerRepository>();
        service.AddScoped<ICustomerWalletRepository, CustomerWalletRepository>();
        service.AddScoped<ICustomerWalletService, CustomerWalletService>();
        return service;
    }
    
}