using API.DTOs.User;
using API.Entities;
using API.Helpers.Paginator;
using API.Helpers.QueryParams;
using API.Interfaces.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public OrderRepository(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<bool> MakeOrderAsync(OrderDto orderDto, int userId)
    {
        using var transaction = await _dataContext.Database.BeginTransactionAsync();
        try
        {
            var order = new Order { AppUserId = userId };
            var orderDishParameters = new List<OrderDishParameter>();

            await _dataContext.Orders.AddAsync(_mapper.Map(orderDto, order));

            if (!(await _dataContext.SaveChangesAsync() > 0))
            {
                await transaction.RollbackAsync();
                return false;
            }

            foreach (var odpd in orderDto.OrderDishParameters)
            {
                var orderDishParameter = new OrderDishParameter { OrderId = order.Id };
                orderDishParameters.Add(_mapper.Map(odpd, orderDishParameter));
            }

            await _dataContext.OrderDishParameters.AddRangeAsync(orderDishParameters);

            if (!(await _dataContext.SaveChangesAsync() > 0))
            {
                await transaction.RollbackAsync();
                return false;
            }

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<PagedList<OrderUserGetDto>> GetUserOrdersAsync(OrderParam orderParam, int userId)
    {
        var source = _dataContext.Orders
            .Where(o => o.AppUserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ProjectTo<OrderUserGetDto>(_mapper.ConfigurationProvider)
            .AsNoTracking();

        return await PagedList<OrderUserGetDto>
            .CreateAsync(source, orderParam.CurrentPage, orderParam.PageSize);
    }

    public void UpdateOrderStatus(int id, string status)
    {
        var order = new Order { Id = id, Status = status };

        _dataContext.Orders.Attach(order);
        _dataContext.Entry(order).Property(o => o.Status).IsModified = true;
    }

    public void UpdateEvaluation(int id, int evaluationValue)
    {
        var order = new Order { Id = id, Evaluation = evaluationValue };

        _dataContext.Orders.Attach(order);
        _dataContext.Entry(order).Property(o => o.Evaluation).IsModified = true;
    }
}