using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopBridge.Api.Controllers;
using ShopBridge.Api.Data;
using ShopBridge.Api.Models;

namespace ShopBridge.Tests
{
    [TestClass]
    public class ProductTests
    {
        private readonly ProductController _productController;

        [ClassInitialize]
        public void SetUp()
        {

            //need to mock setup
        }


        public ProductTests(ProductController productController)
        {
            _productController = productController;
        }


        [TestMethod]
        public void AddItem_ReturnIntegerId_WhenAllvaluesValid()
        {
            var product = new ProductInfoModel()
            {
                Name = "Pen",
                Description = "Blue Pen",
                Price = 10.30M
            };


            var result = _productController.AddItem(product);

            Assert.IsInstanceOfType(result, typeof(int));

        }
    }
}
