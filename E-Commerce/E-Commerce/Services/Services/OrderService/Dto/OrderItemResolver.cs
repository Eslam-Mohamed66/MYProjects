using AutoMapper;
using Core.Entities;
using Core.Entities.OrderEntities;
using Microsoft.Extensions.Configuration;
using Services.Services.ProductService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.OrderService.Dto
{
    public class OrderItemResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration configuration;

        public OrderItemResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
                return $"{configuration["BaseUrl"]}{ source.ItemOrdered.PictureUrl}";

            return null;
        }
    }
}
