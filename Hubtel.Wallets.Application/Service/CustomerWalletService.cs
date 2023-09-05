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

    public bool CustomerWalletExists(Guid customerId, string accountNumber)
    {
        var result = _customerRepository.GetCustomerById(customerId);
        if (result == null)
        {
            var walletExists = result?.Result.CustomerWallets;
            if (walletExists != null)
            {
                var wallet = walletExists.FirstOrDefault(ac => ac.AccountNumber == accountNumber);
            }
            return true;
        }

        return false;
    }
}