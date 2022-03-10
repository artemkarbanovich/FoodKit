using API.DTOs.User;
using API.Helpers.Paginator;
using API.Helpers.QueryParams;

namespace API.Interfaces.Data;

public interface IOrderRepository
{
    Task<bool> MakeOrderAsync(OrderDto orderDto, int userId);
    Task<PagedList<OrderUserGetDto>> GetUserOrdersAsync(OrderParam orderParam, int userId);
    void UpdateOrderStatus(int id, string status);
    void UpdateEvaluation(int id, int evaluationValue);
}