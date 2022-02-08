using API.Data.Repositories;
using API.Interfaces.Data;

namespace API.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;

    public UnitOfWork(DataContext dataContext)
    {
        _dataContext = dataContext;
    }


    public IUserRepository UserRepository => new UserRepository(_dataContext);
    public IPersonalDataRepository PersonalDataRepository => new PersonalDataRepository(_dataContext);


    public async Task<bool> CompleteAsync() => await _dataContext.SaveChangesAsync() > 0;
    public bool HasChanges() => _dataContext.ChangeTracker.HasChanges();
}