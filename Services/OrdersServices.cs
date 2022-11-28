using Domain.CustomExceptions;
using Domain.DataAccess;
using Domain.DTO;
using Domain.Models;
using Domain.Services;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Services;

public class OrdersServices : IOrdersServices
{
    private readonly IUnitOfWork _unitOfWork;

    public OrdersServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GetOrdersResponse> Get(GetOrdersRequest request)
    {
        var from = request.From ?? DefaultFromForGetRequest();
        var to = request.To ?? DefaultToForGetRequest();
        var limit = request.Limit ?? DefaultLimitForGetRequest();

        Expression<Func<Order, bool>> filterExpression;

        if (request.ProviderId is not null)
        {
            filterExpression =
                o => o.Date >= from && o.Date < to
                    && o.Provider.Id == request.ProviderId;
        }
        else
        {
            filterExpression =
                o => o.Date >= from && o.Date < to;
        }

        var orders = await _unitOfWork.OrdersRepository.Get(
            filterExpression,
            q => q.OrderByDescending(o => o.Date),
            $"{nameof(Order.Provider)},{nameof(Order.Items)}",
            limit,
            request.Offset
        );

        var ordersNumber = await _unitOfWork.OrdersRepository.GetOrdersNumber(filterExpression);

        return new GetOrdersResponse(orders, ordersNumber);
    }

    public async Task<Order> GetById(int id)
    {
        return await _unitOfWork.OrdersRepository
            .GetById(id, $"{nameof(Order.Provider)},{nameof(Order.Items)}");
    }

    public async Task Create(NewOrderRequest request)
    {
        Order order = await ConvertToOrder(request);

        await ValidateOrder(order);

        await _unitOfWork.OrdersRepository.Insert(order);
        await _unitOfWork.Save();
    }

    public async Task Update(UpdateOrderRequest request)
    {
        Order order = await GetById(request.Id);
        var itemsIdsToDelete = GetItemsIdsToDeleteOnUpdate(order, request);
        await UpdateOrderByRequest(order, request);

        await ValidateOrder(order);

        await _unitOfWork.OrdersRepository.RemoveItems(itemsIdsToDelete);
        _unitOfWork.OrdersRepository.Update(order);
        await _unitOfWork.Save();
    }

    public async Task Delete(int id)
    {
        var order = await GetById(id);
        await _unitOfWork.OrdersRepository.RemoveItems(order.Items.Select(i => i.Id));
        await _unitOfWork.OrdersRepository.Delete(id);
        await _unitOfWork.Save();
    }

    private int DefaultLimitForGetRequest()
    {
        return 10;
    }

    private DateTime DefaultFromForGetRequest()
    {
        return DateTime.UtcNow.Date.AddMonths(-1);
    }

    private DateTime DefaultToForGetRequest()
    {
        return DateTime.UtcNow.Date.AddDays(1);
    }

    private async Task<Order> ConvertToOrder(NewOrderRequest request)
    {
        Order order = new Order(
            request.Number,
            request.Date.ToUniversalTime(),
            await _unitOfWork.ProvidersRepository.GetById(request.ProviderId),
            request.Items.Select(i => new OrderItem(i.Name, i.Quantity, i.Unit)).ToArray()
        );

        return order;
    }

    private int[] GetItemsIdsToDeleteOnUpdate(Order order, UpdateOrderRequest request)
    {
        return order.Items
            .Select(i => i.Id)
            .Where(i => request.Items.Select(item => item.Id).Contains(i) == false)
            .ToArray();
    }

    private async Task UpdateOrderByRequest(Order order, UpdateOrderRequest request)
    {
        order.Number = request.Number;
        order.Date = request.Date.ToUniversalTime();
        order.Provider = await _unitOfWork.ProvidersRepository.GetById(request.ProviderId);

        foreach (var itemId in GetItemsIdsToDeleteOnUpdate(order, request))
        {
            order.RemoveItem(itemId);
        }

        foreach (var item in request.Items)
        {
            if (item.Id == 0
                || order.Items.Select(i => i.Id).Contains(item.Id) == false)
            {
                order.AddItem(
                    new OrderItem(item.Name, item.Quantity, item.Unit));
                continue;
            }

            order.UpdateItem(
                item.Id,
                new OrderItem(item.Name, item.Quantity, item.Unit));
        }
    }

    private async Task ValidateOrder(Order order)
    {
        List<ValidationResult> validationResults = order.Validate(new ValidationContext(order)).ToList();

        if (await OrderWithSameNumberAndSameProviderExists(order))
        {
            validationResults.Add(
                new ValidationResult(
                    $"Order with same number ({order.Number}) and same provider ({order.Provider.Name}) exists",
                    new[] { nameof(Order.Number), nameof(Order.Provider) }));
        }

        if (validationResults.Any())
        {
            throw new OrderValidationException(validationResults);
        }
    }

    private async Task<bool> OrderWithSameNumberAndSameProviderExists(Order order)
    {
        Expression<Func<Order, bool>> filterExpression =
            o => o.Id != order.Id
                && o.Number == order.Number
                && o.Provider.Id == order.Provider.Id;

        var orders = await _unitOfWork.OrdersRepository.Get(filterExpression);
        
        return orders.Any();
    }
}
