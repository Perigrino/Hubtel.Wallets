using Hubtel.Wallets.Application.Database;
using Hubtel.Wallets.Application.Interface;
using Hubtel.Wallets.Application.Model;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Wallets.Application.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetCustomerAsync()
    {
        var customers = await _context.Customers
            // .Include(wallets => wallets.CustomerWallets )
            .ToListAsync();
        return customers;
    }

    public async Task<Customer> GetCustomerById(Guid id)
    {
        var result = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        return result ?? throw new InvalidOperationException();
    }

    // public async Task<CustomerWallet> GetCustomerWalletsById(Guid customerId)
    // {
    //     var wallets = await _context.CustomerWallets.FirstOrDefaultAsync(w => w.CustomerId == customerId);
    //     return wallets ?? throw new InvalidOperationException();
    // }

    public async Task<bool> CreateCustomer(Customer customer)
    {
        var newCustomer = new Customer()
        {
            Id = Guid.NewGuid(),
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Address = customer.Address,
            Email = customer.Email,
            BirthDate = customer.BirthDate,
            PhoneNumber = customer.PhoneNumber
        };
        await _context.AddAsync(newCustomer);
        return await Save();
    }

    public async Task<bool> UpdateCustomer(Customer customer)
    {
        var result = await _context.Customers.FirstOrDefaultAsync(p => p.Id  == customer.Id);
        if (result != null)
        {
            result.FirstName = customer.FirstName;
            result.LastName = customer.LastName;
            result.Address = customer.Address;
            result.Email = customer.Email;
            result.BirthDate = customer.BirthDate;
            result.PhoneNumber = customer.PhoneNumber;
        }
        return await Save();
    }

    public async Task<bool> DeleteCustomer(Guid id)
    {
        var result = await _context.Customers.FirstOrDefaultAsync(i => i.Id == id);
        if (result == null)
        {
            return false; // Customer not found or already deleted
        }
        _context.Remove(result);
        return await Save();
    }

    public async Task<bool> CustomerExists(Guid id)
    {
        var customer =  await _context.Customers.AnyAsync(c => c.Id == id);
        return customer;
    }
    
    public async Task<bool> Save()
    {
        var saved =  await _context.SaveChangesAsync();
        return saved > 0;
    }
}