using Hubtel.Wallets.Application.Model;

namespace Hubtel.Wallets.Application.Interface;

public interface ICustomerWalletRepository
{
    Task<IEnumerable<CustomerWallet>> GetCustomerWalletsAsync();
    Task<CustomerWallet> GetWalletByWalletId(Guid walletId);
    Task<bool> CreateCustomerWallet(CustomerWallet wallet);
    Task<bool> UpdateCustomerWallet(CustomerWallet wallet);
    Task<bool> DeleteCustomerWallet(Guid walletId);
    Task<bool> WalletExists(Guid walletId);
    Task<bool> Save();
}