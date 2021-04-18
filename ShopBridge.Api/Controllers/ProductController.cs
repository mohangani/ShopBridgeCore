using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBridge.Api.Data;
using ShopBridge.Api.Models;
using ShopBridge.Api.Validators;

namespace ShopBridge.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductDataAccess _productDataAcces;

        public readonly ProductValidator _productValidator;

        public readonly ProductModifyValidator _productModifyValidator;

        public ProductController(IProductDataAccess productDataAcces, ProductValidator productValidator, ProductModifyValidator productModifyValidator)
        {
            _productDataAcces = productDataAcces;
            _productValidator = productValidator;
            _productModifyValidator = productModifyValidator;
        }

        [HttpPost("AddItem")]
        public async Task<ActionResult> AddItem(ProductInfoModel productInfo)
        {
            //validations
            var validation = await _productValidator.ValidateAsync(productInfo);

            if (validation.IsValid)
                return Ok(await _productDataAcces.AddItem(productInfo));

            return BadRequest(validation.Errors);
        }

        [HttpPost("ModifyItem")]
        public async Task<ActionResult> ModifyItem(ProductInfoModel productInfo)
        {
            //validations
            var validation = await _productModifyValidator.ValidateAsync(productInfo);

            if (validation.IsValid)
                return Ok(await _productDataAcces.ModifyItem(productInfo));

            return BadRequest(validation.Errors);
        }

        [HttpDelete("DeleteItem")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            //validations
            if (id <= 0 || !await _productDataAcces.IsProductIdExists(id))
                return BadRequest("Product Item Id Not Valid.");

            return Ok(await _productDataAcces.DeleteItem(id));
        }

        [HttpGet("ProductItemsList")]
        public async Task<ActionResult> ProductItemsList()
        {
            return Ok(await _productDataAcces.ProductItemsList());
        }
    }
}
