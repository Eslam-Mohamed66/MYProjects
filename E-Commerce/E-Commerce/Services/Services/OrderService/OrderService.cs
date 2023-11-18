using AutoMapper;
using Core.Entities;
using Core.Entities.OrderEntities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Specifications;
using Services.Services.BasketService;
using Services.Services.BasketService.Dto;
using Services.Services.OrderService.Dto;
using Services.Services.PaymentService;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Core.Entities.Product;

namespace Services.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService basketService;
        private readonly IUnitOfWork uniteOfWork;
        private readonly IMapper mapper;
        private readonly IPaymentService paymentService;

        public OrderService(
            IBasketService basketService,
            IUnitOfWork uniteOfWork,
            IMapper mapper,
            IPaymentService paymentService)
        {
            this.basketService = basketService;
            this.uniteOfWork = uniteOfWork;
            this.mapper = mapper;
            this.paymentService = paymentService;
        }
        public async Task<OrderResultDto> CreateOrderAsync(OrderDto orderDto)
        {
            //Get Basket
            var basket = await basketService.GetBasketAsync(orderDto.BasketId);

            if (basket == null)
                return null;

            //Fill OrderItems from basket items
            var orderItems = new List<OrderItemDto>();

            foreach (var item in basket.BasketItems)
            {
                var productItem = await uniteOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered =  new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(productItem.Price, item.Quantity, itemOrdered);

                var mappedOrderItem = mapper.Map<OrderItemDto>(orderItem);

                orderItems.Add(mappedOrderItem);
            }

            //Get Delivery Method
            var deliveryMethod = await uniteOfWork.Repository<DeliveryMethod>().GetByIdAsync(orderDto.DeliveryMethodId);    

            //Get Subtotal
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            //ToDo => Check if order exist
            var specs = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
            var existingOrder = await uniteOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);

            CustomerBasketDto customerBasket = new CustomerBasketDto();
            if(existingOrder != null)
            {
                uniteOfWork.Repository<Order>().Delete(existingOrder);
                await paymentService.CreateOrUpdatePaymentIntentForExistingOrder(basket);
            }
            else
            {
                customerBasket = await paymentService.CreateOrUpdatePaymentIntentForNewOrder(basket.Id);

            }

            //Create order
            var mappedShippingAddress = mapper.Map<ShippingAddress>(orderDto.ShippingAddress);
            var mappedOrderItems = mapper.Map<List<OrderItem>>(orderItems);


            var order = new Order(orderDto.BuyerEmail, mappedShippingAddress, deliveryMethod, mappedOrderItems, subTotal , basket.PaymentIntentId ??customerBasket.PaymentIntentId);

            await uniteOfWork.Repository<Order>().Add(order);

            await uniteOfWork.Complete();

            //Delete basket
            //await basketService.DeleteBasketAsync(orderDto.BasketId);

            var mappedOrder = mapper.Map<OrderResultDto>(order);

            return mappedOrder;

        }

        public async Task<OrderResultDto> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var specs = new OrderWithItemsSpecification(id , buyerEmail);

            var order = await uniteOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);

            var mappedOrder = mapper.Map<OrderResultDto>(order);

            return mappedOrder;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodsAsync()
        => await uniteOfWork.Repository<DeliveryMethod>().GetAllAsync();

        public async Task<IReadOnlyList<OrderResultDto>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var specs = new OrderWithItemsSpecification(buyerEmail);

            var orders = await uniteOfWork.Repository<Order>().GetAllWithSpecificationsAsync(specs);

            var mappedOrders = mapper.Map<List<OrderResultDto>>(orders);

            return mappedOrders;
        }
    }
}
