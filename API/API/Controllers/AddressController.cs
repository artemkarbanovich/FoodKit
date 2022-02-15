using API.DTOs.User;
using API.Entities;
using API.Extensions;
using API.Interfaces.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class AddressController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddressController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    [HttpPost("add-address")]
    public async Task<ActionResult<int>> AddAddress(AddressDto addressDto)
    {
        if (await _unitOfWork.AddressRepository.GetAddressesCountByUserIdAsync(User.GetUserId()) >= 5)
            return BadRequest("Количество адресов не может быть больше 5");

        var address = new Address { AppUserId = User.GetUserId() };
        _mapper.Map(addressDto, address);

        await _unitOfWork.AddressRepository.AddAddressAsync(address);

        if (await _unitOfWork.CompleteAsync())
            return address.Id;

        return BadRequest("Ошибка добавления адреса");
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