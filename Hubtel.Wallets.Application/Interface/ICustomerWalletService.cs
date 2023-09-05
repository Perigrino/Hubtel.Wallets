namespace Hubtel.Wallets.Application.Interface;

public interface ICustomerWalletService
{
    bool HasReachedMaxWallets(Guid customerId);
}