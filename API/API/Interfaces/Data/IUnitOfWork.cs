namespace API.Interfaces.Data;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IPersonalDataRepository PersonalDataRepository { get; }
    IAddressRepository AddressRepository { get; }

    Task<bool> CompleteAsync();
    bool HasChanges();
}