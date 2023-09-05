namespace Hubtel.Wallets.Application.Interface;

public interface ICustomerWalletService
{
    bool HasReachedMaxWallets(Guid customerId);
    bool CustomerWalletExists(Guid customerId, string accountNumber);
}