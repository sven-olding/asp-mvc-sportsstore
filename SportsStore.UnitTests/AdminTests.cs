using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            // arrange 
            Mock<IProductRepository> mock = getMockProductRepository();
            AdminController controller = new AdminController(mock.Object);
            // act
            Product[] result = ((IEnumerable<Product>)controller.Index().ViewData.Model).ToArray();
            // assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0].Name, "P1");
            Assert.AreEqual(result[1].Name, "P2");
            Assert.AreEqual(result[2].Name, "P3");
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            // arrange
            Mock<IProductRepository> mock = getMockProductRepository();
            AdminController controller = new AdminController(mock.Object);
            // act 
            Product p1 = controller.Edit(1).ViewData.Model as Product;
            Product p2 = controller.Edit(2).ViewData.Model as Product;
            Product p3 = controller.Edit(3).ViewData.Model as Product;
            // assert
            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            // arrange
            Mock<IProductRepository> mock = getMockProductRepository();
            AdminController controller = new AdminController(mock.Object);
            // act
            Product p = controller.Edit(4).ViewData.Model as Product;
            // assert
            Assert.IsNull(p);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // arrange
            Mock<IProductRepository> mock = getMockProductRepository();
            AdminController controller = new AdminController(mock.Object);
            Product product = new Product { Name = "Test" };

            // act
            ActionResult result = controller.Edit(product);

            // assert
            mock.Verify(m => m.SaveProduct(product));           
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // arrange
            Mock<IProductRepository> mock = getMockProductRepository();
            AdminController controller = new AdminController(mock.Object);
            Product product = new Product { Name = "Test" };
            controller.ModelState.AddModelError("error", "error");

            // act
            ActionResult result = controller.Edit(product);

            // assert
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        private Mock<IProductRepository> getMockProductRepository()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" },
                new Product { ProductID = 3, Name = "P3"}
            });
            return mock;
        }
    }
}
