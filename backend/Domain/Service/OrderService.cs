using backend.Domain.Contract;
using backend.Domain.DTO;
using backend.Domain.Entity;
using backend.Domain.Repository;

namespace backend.Domain.Service
{
    public class OrderService(IOrderRepository orderRepository, IItemRepository itemRepository) : IOrderService
    {
        private async Task<OrderObject> BuildOrderObjectAsync(Order order)
        {
            var orderStrings = await orderRepository.GetStringsByOrderIdAsync(order.Id);
            return new OrderObject
            {
                OrderId = order.Id,
                Creation = order.Creation,
                Execution = order.Execution,
                Status = order.Status,
                OrderStrings = orderStrings
            };
        }

        private async Task ValidateOrderStringsAsync(List<OrderStringRequest> orderStrings)
        {
            foreach (var orderString in orderStrings)
            {
                if (orderString.Count < 1)
                {
                    throw new InvalidOperationException("Количество в строке заказа должно быть не меньше 1.");
                }

                var childIds = await itemRepository.GetChildrenIdByRootId(orderString.ItemId);
                if (childIds.Count == 0)
                {
                    throw new InvalidOperationException("Материал нижнего уровня нельзя добавлять в заказ.");
                }
            }
        }

        public async Task<int> CreateOrderObjectAsync(OrderRequest request)
        {
            await ValidateOrderStringsAsync(request.OrderStrings);

            var orderId = await orderRepository.CreateAsync(new Order
            {
                Creation = request.Creation,
                Execution = request.Execution,
                Status = request.Status
            });

            foreach (var orderString in request.OrderStrings)
            {
                await orderRepository.CreateStringAsync(new OrderString
                {
                    OrderId = orderId,
                    ItemId = orderString.ItemId,
                    Count = orderString.Count
                });
            }

            return orderId;
        }

        public async Task<List<OrderObject>> GetAllOrderObjectsAsync()
        {
            List<OrderObject> orderObjects = [];
            var orders = await orderRepository.GetAsync();

            foreach (var order in orders)
                orderObjects.Add(await BuildOrderObjectAsync(order));
            return orderObjects;
        }

        public async Task<int> UpdateOrderObjectAsync(int orderId, OrderRequest request)
        {
            await ValidateOrderStringsAsync(request.OrderStrings);

            await orderRepository.UpdateAsync(orderId, request.Creation, request.Execution, request.Status);
            foreach (var orderString in (await orderRepository.GetStringsByOrderIdAsync(orderId)))
                await orderRepository.DeleteStringAsync(orderString.Id);

            foreach (var orderString in request.OrderStrings)
            {
                await orderRepository.CreateStringAsync(new OrderString
                {
                    OrderId = orderId,
                    ItemId = orderString.ItemId,
                    Count = orderString.Count
                });
            }

            return orderId;
        }

        public async Task<int> DeleteOrderObjectAsync(int orderId)
        {
            var orderStrings = await orderRepository.GetStringsByOrderIdAsync(orderId);
            foreach (var orderString in orderStrings)
                await orderRepository.DeleteStringAsync(orderString.Id);
            return await orderRepository.DeleteAsync(orderId);
        }
    }
}
