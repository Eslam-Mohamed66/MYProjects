using AutoMapper;
using Core.Entities;
using Core.Entities.OrderEntities;
using Infrastructure.Interfaces;
using Infrastructure.Specifications;
using Microsoft.Extensions.Configuration;
using Services.Services.BasketService;
using Services.Services.BasketService.Dto;
using Services.Services.OrderService.Dto;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Core.Entities.Product;

namespace Services.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBasketService basketService;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public PaymentService(
            IUnitOfWork unitOfWork,
            IBasketService basketService,
            IConfiguration configuration,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.basketService = basketService;
            this.configuration = configuration;
            this.mapper = mapper;
        }
        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForExistingOrder(CustomerBasketDto? basket)
        {
            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];

            if (basket == null)
                return null;

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Price;
            }

            foreach(var item in basket.BasketItems)
            {
                var productItem = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                if(item.Price != productItem.Price)
                    item.Price = productItem.Price;
            }

            var service = new PaymentIntentService();

            PaymentIntent intent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var option = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + ((long)shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                intent = await service.CreateAsync(option);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var option = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + (long)shippingPrice * 100,
                };
                await service.UpdateAsync(basket.PaymentIntentId, option);
            }

            await basketService.UpdateBasketAsync(basket);

            return basket;
        }

        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForNewOrder(string basketId)

        {
            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];

            var basket = await basketService.GetBasketAsync(basketId);

            if (basket == null)
                return null;

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in basket.BasketItems)
            {
                var productItem = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                if (item.Price != productItem.Price)
                    item.Price = productItem.Price;
            }

            var service = new PaymentIntentService();

            PaymentIntent intent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var option = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + ((long)shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                intent = await service.CreateAsync(option);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var option = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + (long)shippingPrice * 100,
                };
                await service.UpdateAsync(basket.PaymentIntentId, option);
            }

            await basketService.UpdateBasketAsync(basket);

            return basket;
        }

        public async Task<OrderResultDto> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var specs = new OrderWithPaymentIntentSpecification(paymentIntentId);
            var order = await unitOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);

            if (order is null)
                return null;

            order.OrderStatus = OrderStatus.PaymentFailed;

            unitOfWork.Repository<Order>().Update(order);

            await unitOfWork.Complete();

            var mappedOrder = mapper.Map<OrderResultDto>(order);

            return mappedOrder;
        }

        public async Task<OrderResultDto> UpdateOrderPaymentSucceeded(string paymentIntentId , string basketId)
        {
            var specs = new OrderWithPaymentIntentSpecification(paymentIntentId);
            var order = await unitOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);

            if (order is null)
                return null;

            order.OrderStatus = OrderStatus.PaymentRecived;

            unitOfWork.Repository<Order>().Update(order);

            await unitOfWork.Complete();

            await basketService.DeleteBasketAsync(basketId);

            var mappedOrder = mapper.Map<OrderResultDto>(order);

            return mappedOrder;
        }
    }
}
