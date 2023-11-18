using Core.Entities;
using Demo.HandleResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services.OrderService;
using Services.Services.OrderService.Dto;
using System.Security.Claims;

namespace Demo.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        [HttpPost]
        public async Task<ActionResult<OrderResultDto>> CreateOrderAsync(OrderDto orderDto)
        {
            var order = await orderService.CreateOrderAsync(orderDto);

            if (order is null)
                return BadRequest(new ApiResponse(400, "Erro While creating your order!!"));

            return Ok(order);
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderResultDto>>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);

            var orders = await orderService.GetAllOrdersForUserAsync(buyerEmail);

            if (orders is { Count: <= 0 })
                return BadRequest(new ApiResponse(200, "You Don't Have Any Orders Yet"));
            
            return Ok(orders);
        }
        [HttpGet]
        public async Task<ActionResult<OrderResultDto>> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);

            var order = await orderService.GetOrderByIdAsync(id , buyerEmail);

            if (order is null)
                return BadRequest(new ApiResponse(200, $"There is no Order With Id {id}"));

            return Ok(order);
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetAllDeliveryMethodsAsync()
            => Ok(await orderService.GetAllDeliveryMethodsAsync());
    }
}
