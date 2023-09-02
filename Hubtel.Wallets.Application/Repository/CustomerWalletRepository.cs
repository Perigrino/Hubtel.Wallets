using Hubtel.Wallets.Application.Database;
using Hubtel.Wallets.Application.Interface;
using Hubtel.Wallets.Application.Model;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Wallets.Application.Repository;

public class CustomerWalletRepository : ICustomerWalletRepository
{
    private readonly AppDbContext _context;

    public CustomerWalletRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CustomerWallet>> GetCustomerWalletsAsync()
    {
        var wallet = await _context.CustomerWallets.OrderBy(createdAt => createdAt.CreatedAt).ToListAsync();
        return wallet;
    }

    public async Task<CustomerWallet> GetWalletByWalletId(Guid walletId)
    {
        var result = await _context.CustomerWallets.FirstOrDefaultAsync(wallet => wallet.Id == walletId);
        return result ?? throw new InvalidOperationException();
    }

    public async Task<bool> CreateCustomerWallet(CustomerWallet wallet)
    {
        var newWallet = new CustomerWallet
        {
            Id = Guid.NewGuid(),
            WalletName = wallet.WalletName,
            Type = wallet.Type,
            AccountNumber = wallet.AccountNumber,
            AccountScheme = wallet.AccountScheme,
            CreatedAt = DateTime.UtcNow,
            Owner = wallet.Owner,
            CustomerId = wallet.CustomerId
        };
        await _context.AddAsync(newWallet);
        return await Save();

    }

    public async Task<bool> UpdateCustomerWallet(CustomerWallet wallet)
    {
        var result = await _context.CustomerWallets.FirstOrDefaultAsync(id => id.Id == wallet.Id);
        if (result != null)
        {
            result.WalletName = wallet.WalletName;
            result.Type = wallet.Type;
            result.AccountNumber = wallet.AccountNumber;
            result.AccountScheme = wallet.AccountScheme;
            result.CreatedAt = DateTime.UtcNow;
            result.Owner = wallet.Owner;
            result.CustomerId = wallet.CustomerId;
        }
        return await Save();
    }

    public async Task<bool> DeleteCustomerWallet(Guid walletId)
    {
        var result = await _context.CustomerWallets.FirstOrDefaultAsync(id => id.Id == walletId);
        if (result == null)
        {
            return false; // Wallet not found or already deleted
        }
        _context.Remove(result);
        return await Save();
    }

    public async Task<bool> WalletExists(Guid id)
    {
        var wallet =  await _context.CustomerWallets.AnyAsync(w => w.Id == id);
        return wallet;
    }
    
    public async Task<bool> Save()
    {
        var saved =  await _context.SaveChangesAsync();
        return saved > 0;
    }
}