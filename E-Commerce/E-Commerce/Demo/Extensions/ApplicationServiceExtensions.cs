using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Demo.HandleResponses;
using Services.Interfaces;
using Services.Services.ProductService;
using Services.Services.ProductService.Dto;
using Services.Services.CacheService;
using Services.Services.BasketService.Dto;
using Services.Services.BasketService;
using Infrastructure.BasketRepository;
using Services.Services.UserService;
using Services.Services.TokenService;
using Services.Services.OrderService;
using Services.Services.PaymentService;
using Services.Services.OrderService.Dto;

namespace Demo.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(model => model.Value.Errors.Count > 0)
                        .SelectMany(model => model.Value.Errors)
                        .Select(error => error.ErrorMessage).ToList();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };


                    return new BadRequestObjectResult(errorResponse);

                };
            });

            //builder.Services.AddAutoMapper(x => x.AddProfile(new ProductProfile()));
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(BasketProfile));
            services.AddAutoMapper(typeof(OrderProfile));

            return services;

        }
    }
}
