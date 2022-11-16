using Domain.DataAccess;
using Domain.DTO;
using Domain.Models;
using Domain.Services;

namespace Services;

public class OrdersServices : IOrdersServices
{
    private readonly IUnitOfWork _unitOfWork;

    public OrdersServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Order>> GetAll()
    {
        return await _unitOfWork.OrdersRepository
            .GetAll(
                orderBy: q => q.OrderByDescending(o => o.Date),
                includeProperties: $"{nameof(Order.Provider)},{nameof(Order.Items)}");
    }

    public async Task<Order> GetById(int id)
    {
        return await _unitOfWork.OrdersRepository
            .GetById(id, $"{nameof(Order.Provider)},{nameof(Order.Items)}");
    }

    public async Task Create(NewOrderRequest request)
    {
        Order order = await ConvertToOrder(request);
        await _unitOfWork.OrdersRepository.Insert(order);
        await _unitOfWork.Save();
    }

    public async Task Update(UpdateOrderRequest request)
    {
        Order order = await GetById(request.Id);
        var itemsIdsToDelete = GetItemsIdsToDeleteOnUpdate(order, request);
        await UpdateOrderByRequest(order, request);
        await _unitOfWork.OrdersRepository.RemoveItems(itemsIdsToDelete);
        _unitOfWork.OrdersRepository.Update(order);
        await _unitOfWork.Save();
    }

    public async Task Delete(int id)
    {
        await _unitOfWork.OrdersRepository.Delete(id);
        await _unitOfWork.Save();
    }

    private async Task<Order> ConvertToOrder(NewOrderRequest request)
    {
        Order order = new Order(
            request.Number,
            request.Date,
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
        order.Date = request.Date;
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
}
