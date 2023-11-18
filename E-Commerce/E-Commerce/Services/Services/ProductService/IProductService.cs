using Core.Entities;
using Infrastructure.Specifications;
using Services.Helper;
using Services.Services.ProductService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductResultDto> GetProductByIdAsync(int? id);
        Task<Pagination<ProductResultDto>> GetProductsAsync(ProductSpecification specification);
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
    }
}

