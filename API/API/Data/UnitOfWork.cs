using API.Data.Repositories;
using API.Interfaces.Data;
using AutoMapper;

namespace API.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UnitOfWork(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public IUserRepository UserRepository => new UserRepository(_dataContext);
    public IPersonalDataRepository PersonalDataRepository => new PersonalDataRepository(_dataContext, _mapper);
    public IAddressRepository AddressRepository => new AddressRepository(_dataContext, _mapper);
    public IUserDishRepository UserDishRepository => new UserDishRepository(_dataContext, _mapper);
    public IIngredientRepository IngredientRepository => new IngredientRepository(_dataContext, _mapper);

    public async Task<bool> CompleteAsync() => await _dataContext.SaveChangesAsync() > 0;
}