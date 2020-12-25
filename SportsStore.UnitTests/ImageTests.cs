using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    /// <summary>
    /// Summary description for ImageTests
    /// </summary>
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            var mock = getMockProductRepository();
            var productController = new ProductController(mock.Object);

            var product = mock.Object.Products.FirstOrDefault(p => p.ProductID == 2);
            ActionResult result = productController.getImage(2);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(((FileResult)result).ContentType, product.ImageMimeType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            var mock = getMockProductRepository();
            var productController = new ProductController(mock.Object);
         
            ActionResult result = productController.getImage(99);
         
            Assert.IsNull(result);
        }

        private Mock<IProductRepository> getMockProductRepository()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name = "P1"},
                new Product { ProductID = 2, Name = "P2", ImageData = new byte[] { }, ImageMimeType = "image/png" },
                new Product { ProductID = 3, Name = "P3"}
            });
            return mock;
        }
    }
}
