using API.Interfaces.Data;
using AutoMapper;

namespace API.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private DataContext _dataContext;
    private IMapper _mapper;

    public OrderRepository(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }
}