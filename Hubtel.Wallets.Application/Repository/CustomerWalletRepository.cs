using Hubtel.Wallets.Application.Database;
using Hubtel.Wallets.Application.Interface;
using Hubtel.Wallets.Application.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Hubtel.Wallets.Application.Repository;

public class CustomerWalletRepository : ICustomerWalletRepository
{
    private readonly AppDbContext _context;

    public CustomerWalletRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CustomerWallet>> GetCustomerWalletsAsync(CancellationToken token = default)
    {
        var wallet = await _context.CustomerWallets
            .OrderBy(createdAt => createdAt.CreatedAt).ToListAsync(cancellationToken: token);
        return wallet;
    }

    public async Task<CustomerWallet> GetWalletByWalletId(Guid walletId , CancellationToken token = default)
    {
        var result = await _context.CustomerWallets
            .FirstOrDefaultAsync(wallet => wallet.Id == walletId, cancellationToken: token);
        return result ?? throw new InvalidOperationException();
    }
    
    public async Task<bool> CreateCustomerWallet(CustomerWallet wallet , CancellationToken token = default)
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
        await _context.AddAsync(newWallet, token);
        return await Save();
    }

    public async Task<bool> UpdateCustomerWallet(CustomerWallet wallet, CancellationToken token = default)
    {
        var result = await _context.CustomerWallets
            .FirstOrDefaultAsync(id => id.Id == wallet.Id, cancellationToken: token);
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

    public async Task<bool> DeleteCustomerWallet(Guid walletId, CancellationToken token = default)
    {
        var result = await _context.CustomerWallets
            .FirstOrDefaultAsync(id => id.Id == walletId, cancellationToken: token);
        if (result == null)
        {
            return false; // Wallet not found or already deleted
        }
        _context.Remove(result);
        return await Save(token);
    }

    public async Task<bool> WalletExists(Guid id, CancellationToken token = default)
    {
        var wallet =  await _context.CustomerWallets
            .AnyAsync(w => w.Id == id, cancellationToken: token);
        return wallet;
    }

    public async Task<bool> CustomerWalletExists(string accountNumber, CancellationToken token = default)
    {
        var wallet =  await _context.CustomerWallets
            .AnyAsync(an => an.AccountNumber == accountNumber, cancellationToken: token);
        return wallet;
    }

    public async Task<bool> Save(CancellationToken token = default)
    {
        var saved =  await _context.SaveChangesAsync(token);
        return saved > 0;
    }
}