using API.Data.Repositories;
using API.Interfaces;
using API.Interfaces.Data;
using AutoMapper;

namespace API.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly IImageService _imageService;

    public UnitOfWork(DataContext dataContext, IMapper mapper, IImageService imageService)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _imageService = imageService;
    }

    public IUserRepository UserRepository => new UserRepository(_dataContext);
    public IPersonalDataRepository PersonalDataRepository => new PersonalDataRepository(_dataContext, _mapper);
    public IAddressRepository AddressRepository => new AddressRepository(_dataContext, _mapper);
    public IUserDishRepository UserDishRepository => new UserDishRepository(_dataContext, _mapper);
    public IIngredientRepository IngredientRepository => new IngredientRepository(_dataContext, _mapper);
    public IDishRepository DishRepository => new DishRepository(_dataContext, _mapper, _imageService);
    public IOrderRepository OrderRepository => new OrderRepository(_dataContext, _mapper);
    public IMessageRepository MessageRepository => new MessageRepository(_dataContext, _mapper);
    
    public async Task<bool> CompleteAsync() => await _dataContext.SaveChangesAsync() > 0;
}