using Hubtel.Wallets.Application.Model;

namespace Hubtel.Wallets.Application.Interface;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetCustomerAsync();
    Task<Customer> GetCustomerById(Guid id);
    //Task<CustomerWallet> GetWalletByCustomerId(Guid id);
    Task<bool> CreateCustomer(Customer customer);
    Task<bool> UpdateCustomer(Customer customer);
    Task<bool> DeleteCustomer(Guid id);
    Task<bool> CustomerExists(Guid id);
    Task<bool> Save();
}