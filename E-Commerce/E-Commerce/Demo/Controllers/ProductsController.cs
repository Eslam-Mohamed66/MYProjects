using Core.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Demo.HandleResponses;
using Services.Helper;
using Services.Interfaces;
using Services.Services.ProductService.Dto;
using Demo.Helper;

namespace Demo.Controllers
{
   
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [Cache(100)]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductResultDto>> GetProductById(int? id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product is null)
                return NotFound(new ApiResponse(404));
            return Ok(product);
        }
           //=> await _productService.GetProductByIdAsync(id);


        [HttpGet]
        [Cache(20)]
        public async Task<ActionResult<Pagination<ProductResultDto>>> GetProducts([FromQuery]ProductSpecification specification)
        {
            var  products = await _productService.GetProductsAsync(specification);

            return Ok(products);
        }
            //=> await _productService.GetProductsAsync(specification);

        [HttpGet]
        [Route("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
            => Ok(await _productService.GetProductBrandsAsync());


        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
           => Ok(await _productService.GetProductTypesAsync());
    }
}
