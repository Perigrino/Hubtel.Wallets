namespace Hubtel.Wallets.Application.Model;

public class CustomerWallet
{
    public required Guid Id { get; set; }
    public required string WalletName { get; set; }
    public required string Type { get; set; }
    public required int AccountNumber { get; set; }
    public required string AccountScheme { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required int Owner { get; set; }
    public Guid customerId { get; set; }
    public Customer Customer { get; set; }

}