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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge.Tests
{
    [TestClass]
    public class ProductItemsListTests
    {

        private readonly ProductController _productController;

        public ProductItemsListTests()
        {

            var productDataAccess = new Mock<IProductDataAccess>();
            productDataAccess.Setup(a => a.ProductItemsList()).ReturnsAsync(new List<ProductInfoModel>() { new ProductInfoModel()});

            var productValidator = new ProductValidator(productDataAccess.Object);
            var productModifyValidator = new ProductModifyValidator(productDataAccess.Object);
            _productController = new ProductController(productDataAccess.Object, productValidator, productModifyValidator);
        }


        [TestMethod]
        public async Task ProductItemsList_Return200_WhenCall()
        {
            var result = await _productController.ProductItemsList();
            var okresult = result as ObjectResult;

            Assert.AreEqual(200, okresult.StatusCode);
        }


        [TestMethod]
        public async Task ProductItemsList_ReturnOneProductItem_WhenCall()
        {
            var result = await _productController.ProductItemsList();
            var okresult = result as ObjectResult;

            Assert.AreEqual(200, okresult.StatusCode);
            Assert.AreEqual(1, ((IEnumerable)okresult.Value).Cast<object>().Count());
        }




    }
}
