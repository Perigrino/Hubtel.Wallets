using Hubtel.Wallets.Application.Interface;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Hubtel.Wallets.Application.Service;

public class CustomerWalletService : ICustomerWalletService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerWalletService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public bool HasReachedMaxWallets(Guid customerId)
    {
        var wallets = _customerRepository.GetCustomerById(customerId);
        if (wallets == null)
        {
            throw new Exception("Customer not found");
        }
        var numberOfWallet = wallets.Result.CustomerWallets.Count;
        
        return numberOfWallet >= 5;
    }
}