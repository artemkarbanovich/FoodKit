﻿namespace API.Interfaces.Data;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IPersonalDataRepository PersonalDataRepository { get; }
    IAddressRepository AddressRepository { get; }
    IUserDishRepository UserDishRepository { get; }
    IIngredientRepository IngredientRepository { get; }

    Task<bool> CompleteAsync();
}