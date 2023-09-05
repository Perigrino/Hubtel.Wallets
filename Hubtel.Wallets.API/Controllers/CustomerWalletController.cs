using Hubtel.Wallets.Application.Interface;
using Hubtel.Wallets.ContractMappings;
using Hubtel.Wallets.Contracts.Request;
using Hubtel.Wallets.Contracts.Response;
using Microsoft.AspNetCore.Mvc;
using TouchGrassCart.Contracts.Response;

namespace Hubtel.Wallets.Controllers;


[ApiController]
public class CustomerWalletController : Controller
{
    private readonly ICustomerWalletRepository _walletRepository;
    private readonly ICustomerWalletService _walletService;

    public CustomerWalletController(ICustomerWalletRepository walletRepository, ICustomerWalletService walletService)
    {
        _walletRepository = walletRepository;
        _walletService = walletService;
    }
    
    //GET all Wallets
    [HttpGet(ApiEndpoints.CustomerWallet.GetAll)]
    public async Task<IActionResult> GetCustomerWallets()
    {
        var wallets = await _walletRepository.GetCustomerWalletsAsync();
        var walletResponse = new FinalResponse<CustomerWalletsResponse>
        {
            StatusCode = 200,
            Message = "Wallets retrieved successfully.",
            Data = wallets.MapsToResponse()
        };
        return Ok(walletResponse);
    }
    
    //GET WalletByWalletsId
    [HttpGet(ApiEndpoints.CustomerWallet.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var wallet = await _walletRepository.GetWalletByWalletId(id);
        if (wallet == null)
        {
            return NotFound(new FinalResponse<object>
            {
                StatusCode = 404,
                Message = "Wallet not found."
            });
        }
        
        var customerResponse = new FinalResponse<CustomerWalletResponse>
        {
            StatusCode = 200,
            Message = "Wallet retrieved successfully.",
            Data = wallet.MapsToResponse()
        };
        return Ok(customerResponse);
    }
    
    //POST Wallet
    [HttpPost(ApiEndpoints.CustomerWallet.Create)]
    public async Task<IActionResult> CreateCustomerWallet([FromBody] CreateCustomerWalletRequest request)
    {
        if (request == null)
        {
            return BadRequest(new FinalResponse<object>() { StatusCode = 400,Message = "Wallet data is invalid." });
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(new FinalResponse<object> { StatusCode = 400, Message = "Validation failed.", Data = ModelState });
        }

        var maxedWalletsReached = _walletService.HasReachedMaxWallets(request.CustomerId);
        if (!maxedWalletsReached)
        {
            var mapToWallet = request.MapToWallet();
            await _walletRepository.CreateCustomerWallet(mapToWallet ?? throw new InvalidOperationException());
            var walletResponse = new FinalResponse<CustomerWalletResponse>
            {
                StatusCode = 201,
                Message = "Wallet created successfully.",
                Data = mapToWallet.MapsToResponse()
            };
            return CreatedAtAction(nameof(Get), new { id = mapToWallet.Id }, walletResponse);
        }

        return BadRequest("User has more than 5 wallets");
    }
    
    //UPDATE Customer
    [HttpPut(ApiEndpoints.CustomerWallet.Update)]
    public async Task<IActionResult> UpdateCustomerWallet([FromRoute] Guid id, [FromBody] UpdateCustomerWalletRequest request)
    {
        if (request == null)
        {
            return BadRequest(new FinalResponse<object>() { StatusCode = 400,Message = "Wallet data is invalid." });
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(new FinalResponse<object> { StatusCode = 400, Message = "Validation failed.", Data = ModelState });
        }
        var mapToWallet = request.MapToWallet(id);
        var updateWallet = await _walletRepository.UpdateCustomerWallet(mapToWallet);
        if (updateWallet is false)
        {
            return NotFound(new FinalResponse<object>
            {
                StatusCode = 404,
                Message = "Wallet not found."
            });
        }
        var response = new FinalResponse<CustomerWalletResponse>
        {
            StatusCode = 200,
            Message = "Customer details updated successfully.",
            Data = mapToWallet.MapsToResponse()
        };
        return Ok(response);

    }

    //DELETE Customer 
    [HttpDelete(ApiEndpoints.CustomerWallet.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _walletRepository.WalletExists(id);
        var deleteCustomerWallet = await _walletRepository.DeleteCustomerWallet(id);
        if (!deleteCustomerWallet)
        {
            return NotFound(new FinalResponse<string>
            {
                StatusCode = 404,
                Message = "Customer wallet not found or already deleted",
                Data = null
            });
        }
        
        return Ok(new FinalResponse<string>
            {
                StatusCode = 200,
                Message = "Customer wallet deleted successfully",
                Data = null
            });
    }
}