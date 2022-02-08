using API.DTOs.User;
using API.Entities;
using API.Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly DataContext _dataContext;

    public AddressRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
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
            .Select(a => new AddressDto
            {
                Id = a.Id,
                Country = a.Country,
                Locality = a.Locality,
                Street = a.Street,
                HouseNumber = a.HouseNumber,
                ApartmentNumber = a.ApartmentNumber,
                EntranceNumber = a.EntranceNumber ?? null,
                Floor = a.Floor ?? null
            })
            .ToListAsync();
    }

    public async Task<Address> GetAddressByIdAsync(int addressId)
    {
        return await _dataContext.Addresses.SingleOrDefaultAsync(a => a.Id == addressId);
    }
}