using API.DTOs.User;

namespace API.Interfaces.Data;

public interface IOrderRepository
{
    Task<bool> MakeOrderAsync(OrderDto orderDto, int userId);
}