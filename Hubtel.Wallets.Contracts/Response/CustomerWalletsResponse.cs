namespace Hubtel.Wallets.Contracts.Response;

public class CustomerWalletsResponse
{
    public required IEnumerable<CustomerWalletResponse> Wallet { get; init; } = Enumerable.Empty<CustomerWalletResponse>();
}