using API.DTOs.User;
using API.Entities;
using API.Interfaces.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public AddressRepository(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }


    public async Task AddAddressAsync(Address address)
    {
        await _dataContext.Addresses.AddAsync(address);
    }

    public void DeleteAddress(Address address)
    {
        _dataContext.Addresses.Remove(address);
    }

    public async Task<List<AddressDto>> GetUserAddressesByUserIdAsync(int userId)
    {
        return await _dataContext.Addresses
            .Where(a => a.AppUserId == userId)
            .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<Address> GetAddressByIdAsync(int addressId)
    {
        return await _dataContext.Addresses.SingleOrDefaultAsync(a => a.Id == addressId);
    }
}