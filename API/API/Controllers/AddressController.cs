using API.DTOs.User;
using API.Entities;
using API.Extensions;
using API.Interfaces.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class AddressController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;

    public AddressController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    [HttpPost("add-address")]
    public async Task<ActionResult<int>> AddAddress(AddressDto addressDto)
    {
        var address = new Address
        {
            AppUserId = User.GetUserId(),
            Country = addressDto.Country,
            Locality = addressDto.Locality,
            Street = addressDto.Street,
            HouseNumber = addressDto.HouseNumber,
            ApartmentNumber = addressDto.ApartmentNumber,
            EntranceNumber = (addressDto.EntranceNumber != null) ? addressDto.EntranceNumber : null,
            Floor = (addressDto.Floor != null) ? addressDto.Floor : null
        };

        await _unitOfWork.AddressRepository.AddAddressAsync(address);

        if (await _unitOfWork.CompleteAsync())
            return address.Id;

        return BadRequest("Ошибка доавления адреса");
    }

    [HttpDelete("delete-address/{addressId}")]
    public async Task<ActionResult> DeleteAddress(int addressId)
    {
        var address = await _unitOfWork.AddressRepository.GetAddressByIdAsync(addressId);

        if (address == null)
            return NotFound("Ошибка удаления адреса");

        _unitOfWork.AddressRepository.DeleteAddress(address);

        if (await _unitOfWork.CompleteAsync())
            return Ok();

        return BadRequest("Ошибка удаления адреса");
    }

    [HttpGet("get-addresses")]
    public async Task<ActionResult<List<AddressDto>>> GetAddresses()
    {
        return await _unitOfWork.AddressRepository.GetUserAddressesByUserIdAsync(User.GetUserId());
    }
}