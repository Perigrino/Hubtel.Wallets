using Hubtel.Wallets.Application.Model;

namespace Hubtel.Wallets.Contracts.Response;

public class CustomersResponse
{
    public required IEnumerable<CustomerResponse> Customers { get; init; } = Enumerable.Empty<CustomerResponse>();
}