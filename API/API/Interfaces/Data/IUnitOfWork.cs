namespace API.Interfaces.Data;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IPersonalDataRepository PersonalDataRepository { get; }
    IAddressRepository AddressRepository { get; }
    IUserDishRepository UserDishRepository { get; }
    IIngredientRepository IngredientRepository { get; }
    IDishRepository DishRepository { get; }
    IOrderRepository OrderRepository { get; }
    IMessageRepository MessageRepository { get; }

    Task<bool> CompleteAsync();
}