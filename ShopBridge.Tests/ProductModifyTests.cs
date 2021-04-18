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
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge.Tests
{
    

    [TestClass]
    public class ProductModifyTests
    {
        private readonly ProductController _productController;

        public ProductModifyTests()
        {

            var productDataAccess = new Mock<IProductDataAccess>();
            productDataAccess.Setup(a => a.ValidateName("testItem")).ReturnsAsync(true);
            productDataAccess.Setup(a => a.IsProductIdExists(1)).ReturnsAsync(true);
            productDataAccess.Setup(a => a.IsProductIdExists(100)).ReturnsAsync(false);

            var productValidator = new ProductValidator(productDataAccess.Object);
            var productModifyValidator = new ProductModifyValidator(productDataAccess.Object);
            _productController = new ProductController(productDataAccess.Object, productValidator, productModifyValidator);
        }


        [TestMethod]
        public async Task ModifyItem_Return200_WhenAllvaluesValid()
        {
            var product = new ProductInfoModel()
            {
                Id = 1,
                Name = "testItem",
                Description = "Blue Pen",
                Price = 10.30M
               
            };

            var result = await _productController.ModifyItem(product);
            var okresult = result as ObjectResult;

            Assert.AreEqual(200, okresult.StatusCode);
        }

        [TestMethod]
        public async Task ModifyItem_Return400WithIdValidationMessage_WhenProductIdWrong()
        {
            var product = new ProductInfoModel()
            {
                Id=100,
                Name = "testItem",
                Description = "Blue Pen",
                Price = 10.30M
            };

            var result = await _productController.ModifyItem(product) as ObjectResult;
            
            Assert.AreEqual(400, result.StatusCode);

            Assert.AreEqual("Product Id is Not Exists", result.GetErrorMessage());
        }

        [TestMethod]
        public async Task ModifyItem_Return400WithIdGraterthanZeroValidationMessage_WhenIdLessthanorEqalZero()
        {
            var product = new ProductInfoModel()
            {
                Name = "testItem",
                Description = "Blue Pen",
                Price = -3.00M
            };

            var result = await _productController.ModifyItem(product) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

            Assert.AreEqual("'Id' must be greater than '0'.", result.GetErrorMessage());
        }

        [TestMethod]
        public async Task ModifyItem_Return400WithNameValidationMessages_WhenNameNullEmpty()
        {
            var product = new ProductInfoModel()
            {
                Id=1,
                Name = null,
                Description = "Blue Pen",
                Price = 3.00M
            };

            var result = await _productController.ModifyItem(product) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

            Assert.AreEqual("'Name' must not be empty.", result.GetErrorMessage());
        }
    }
}
