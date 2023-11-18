using Demo.HandleResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services.BasketService.Dto;
using Services.Services.OrderService.Dto;
using Services.Services.PaymentService;
using Stripe;

namespace Demo.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IPaymentService paymentService;
        private readonly ILogger<PaymentController> _logger;
        private const string WhSecret = "whsec_53c0dabfb806354e6ab189d04e18dd3a62f1b9f69adcc4ca3b8acdb7e5561ab3";

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            this.paymentService = paymentService;
            _logger = logger;
        }
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForExistingOrder(CustomerBasketDto basket , string basketId)
        {
             var customerBasket = await paymentService.CreateOrUpdatePaymentIntentForExistingOrder(basket);

            if (customerBasket is null)
                return BadRequest(new ApiResponse(400, "Problem With Your Basket"));

            return Ok(customerBasket);
        }

        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForNewOrder( string basketId)
        {
            var customerBasket = await paymentService.CreateOrUpdatePaymentIntentForNewOrder(basketId);

            if (customerBasket is null)
                return BadRequest(new ApiResponse(400, "Problem With Your Basket"));

            return Ok(customerBasket);
        }

        [HttpPost]
        public async Task<ActionResult> WebHook(string basketId)
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], WhSecret);

                PaymentIntent paymentIntent;
                OrderResultDto order;

                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Failed: " , paymentIntent.Id);  
                    order = await paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                    _logger.LogInformation("Order Updated To Payment Failed: ", paymentIntent.Id);

                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Succeed: ", paymentIntent.Id); 
                    order = await paymentService.UpdateOrderPaymentSucceeded(paymentIntent.Id , basketId);
                    _logger.LogInformation("Order Updated To Payment Succeed: ", paymentIntent.Id);
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

    }
}
