using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShopBridge.Api.Controllers;
using ShopBridge.Api.Data;
using ShopBridge.Api.Models;
using ShopBridge.Api.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge.Tests
{

    [TestClass]
    public class ProductDeleteTests
    {
        private readonly ProductController _productController;

        public ProductDeleteTests()
        {

            var productDataAccess = new Mock<IProductDataAccess>();
            productDataAccess.Setup(a => a.ValidateName("testItem")).ReturnsAsync(true);
            productDataAccess.Setup(a => a.IsProductIdExists(1)).ReturnsAsync(true);

            var productValidator = new ProductValidator(productDataAccess.Object);
            var productModifyValidator = new ProductModifyValidator(productDataAccess.Object);
            _productController = new ProductController(productDataAccess.Object, productValidator, productModifyValidator);
        }


        [TestMethod]
        public async Task DeleteItem_Return200_WhenGivenIdisValid()
        {
            var productId = 1;

            var result = await _productController.DeleteItem(productId) as ObjectResult;

            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task DeleteItem_Return400_WhenGivenIdIsNotValid()
        {
            var productId = 100;

            var result = await _productController.DeleteItem(productId) as ObjectResult;

            Assert.AreEqual(400, result.StatusCode);

            Assert.AreEqual("Product Item Id Not Valid.", result.Value);
        }
    }

   
}
