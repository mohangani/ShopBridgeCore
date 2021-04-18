using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShopBridge.Api.Controllers;
using ShopBridge.Api.Data;
using ShopBridge.Api.Models;
using ShopBridge.Api.Validators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBridge.Tests
{
    [TestClass]
    public class ProductTests
    {
        private readonly ProductController _productController;

        public ProductTests()
        {

            var productDataAccess = new Mock<IProductDataAccess>();
            productDataAccess.Setup(a => a.ValidateName("testItem")).ReturnsAsync(true);
            productDataAccess.Setup(a => a.IsProductIdExists(1)).ReturnsAsync(false);

            var productValidator = new ProductValidator(productDataAccess.Object);
            var productModifyValidator = new ProductModifyValidator(productDataAccess.Object);
            _productController = new ProductController(productDataAccess.Object, productValidator, productModifyValidator);
        }


        [TestMethod]
        public async Task AddItem_Return200_WhenAllvaluesValid()
        {
            var product = new ProductInfoModel()
            {
                Name = "testItem",
                Description = "Blue Pen",
                Price = 10.30M
            };

            var result = await _productController.AddItem(product);
            var okresult = result as ObjectResult;

            Assert.AreEqual(200, okresult.StatusCode);
        }

        [TestMethod]
        public async Task AddItem_Return400WithDuplicateProductMessage_WhenProductNameDuplicate()
        {
            var product = new ProductInfoModel()
            {
                Name = "duplicate",
                Description = "Blue Pen",
                Price = 10.30M
            };

            var result = await _productController.AddItem(product) as ObjectResult;
            Assert.AreEqual(400, result.StatusCode);

            var msg = ((IList)result.Value)[0].GetType().GetProperty("ErrorMessage").GetValue(((IList)result.Value)[0]);

            Assert.AreEqual("Product Name Already Exists", msg);
        }

        [TestMethod]
        public async Task AddItem_Return400WithPriceValidationMessage_WhenPriceLessthanZero()
        {
            var product = new ProductInfoModel()
            {
                Name = "testItem",
                Description = "Blue Pen",
                Price = -3.00M
            };

            var result = await _productController.AddItem(product) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

            var msg = ((IList)result.Value)[0].GetType().GetProperty("ErrorMessage").GetValue(((IList)result.Value)[0]);

            Assert.AreEqual("'Price' must be greater than '0'.", msg);
        }

        [TestMethod]
        public async Task AddItem_Return400WithNameValidationMessages_WhenNameNullEmpty()
        {
            var product = new ProductInfoModel()
            {
                Name = null,
                Description = "Blue Pen",
                Price = 3.00M
            };

            var result = await _productController.AddItem(product) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

            var msg = ((IList)result.Value)[0].GetType().GetProperty("ErrorMessage").GetValue(((IList)result.Value)[0]);

            Assert.AreEqual("'Name' must not be empty.", msg);
        }
    }
}
