using FluentValidation;
using ShopBridge.Api.Data;
using ShopBridge.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Api.Validators
{
    public class ProductValidator : AbstractValidator<ProductInfoModel>
    {
        public ProductValidator(IProductDataAccess _productDa)
        {
            RuleFor(product => product.Name).NotNull().NotEmpty().MustAsync(async (productname, cancellation) =>
            {
                return await _productDa.ValidateName(productname);
            }).WithMessage("Product Name Already Exists");

            RuleFor(product => product.Price).GreaterThan(0);
        }

    }

}
