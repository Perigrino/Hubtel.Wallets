namespace Hubtel.Wallets.Contracts.Response;

public class CustomerWalletsResponse
{
    public required IEnumerable<CustomerWalletResponse> CustomerWallet { get; init; } = Enumerable.Empty<CustomerWalletResponse>();
}