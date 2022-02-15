using API.DTOs.User;
using API.Entities;

namespace API.Interfaces.Data;

public interface IAddressRepository
{
    Task<List<AddressDto>> GetUserAddressesByUserIdAsync(int userId);
    Task<Address> GetAddressByIdAsync(int addressId);
    Task AddAddressAsync(Address address);
    void DeleteAddress(Address address);
    Task<int> GetAddressesCountByUserId(int userId);
}