namespace Hubtel.Wallets.Contracts.Request;

public class CreateCustomerWalletRequest
{
    public required Guid Id { get; set; }
    public required string WalletName { get; set; }
    public required string Type { get; set; }
    public required int AccountNumber { get; set; }
    public required string AccountScheme { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required int Owner { get; set; }
    public Guid customerId { get; set; }
}