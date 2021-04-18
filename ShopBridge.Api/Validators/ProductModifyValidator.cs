using FluentValidation;
using ShopBridge.Api.Data;
using ShopBridge.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Api.Validators
{
    public class ProductModifyValidator : AbstractValidator<ProductInfoModel>
    {
        public ProductModifyValidator(IProductDataAccess _productDa)
        {
            RuleFor(product => product.Id).GreaterThan(0).MustAsync(async (productId, cancellation) =>
            {
                return await _productDa.IsProductIdExists(productId);
            }).WithMessage("Product Id is Not Exists");

            RuleFor(product => product.Name).NotNull().NotEmpty();

            RuleFor(product => product.Price).GreaterThan(0);

        }
    }
}
