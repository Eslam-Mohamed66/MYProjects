using Services.Services.BasketService.Dto;
using Services.Services.OrderService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForExistingOrder(CustomerBasketDto basket);
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForNewOrder(string basketId);
        Task<OrderResultDto> UpdateOrderPaymentSucceeded(string paymentIntentId , string basketId);
        Task<OrderResultDto> UpdateOrderPaymentFailed(string paymentIntentId);
    }
}
