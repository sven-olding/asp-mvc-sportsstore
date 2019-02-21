using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    /// <summary>
    /// Summary description for CartTests
    /// </summary>
    [TestClass]
    public class CartTests
    {

        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Cart cart = new Cart();
            // act
            cart.AddItem(p1, 2);
            cart.AddItem(p2, 1);
            CartLine[] results = cart.Lines.ToArray();
            // assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product, p2);
            Assert.AreEqual(results[0].Quantity, 2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Cart cart = new Cart();
            // act
            cart.AddItem(p1, 2);
            cart.AddItem(p2, 1);
            cart.AddItem(p2, 3);
            CartLine[] results = cart.Lines.ToArray();
            // assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 2);
            Assert.AreEqual(results[1].Quantity, 4);
        }


        [TestMethod]
        public void Can_Remove_Line()
        {
            // arrange
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };
            Cart cart = new Cart();
            // act
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 2);
            cart.AddItem(p3, 4);
            cart.RemoveItem(p2);
            // assert
            Assert.AreEqual(cart.Lines.Where(p => p.Product == p2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // arrange
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            Cart cart = new Cart();
            // act
            cart.AddItem(p1, 2);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 1);
            decimal result = cart.ComputeTotalValue();
            // assert
            Assert.AreEqual(result, 350M);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // arrange
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            Cart cart = new Cart();
            // act
            cart.AddItem(p1, 2);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 1);
            cart.Clear();
            // assert
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // arrange
            Mock<IProductRepository> repo = getMockProductRepository();
            CartController cartController = new CartController(repo.Object);
            Cart cart = new Cart();
            // act
            cartController.AddToCart(cart, 1, null);
            // assert
            Assert.AreEqual(1, cart.Lines.Count());
            Assert.AreEqual(1, cart.Lines.ToArray()[0].Product.ProductID);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            // arrange
            Mock<IProductRepository> repo = getMockProductRepository();
            CartController cartController = new CartController(repo.Object);
            Cart cart = new Cart();
            // act
            RedirectToRouteResult result = cartController.AddToCart(cart, 2, "myUrl");
            // assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("myUrl", result.RouteValues["returnUrl"]);
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // arrange
            Cart cart = new Cart();
            CartController controller = new CartController(null);
            // act
            CartIndexViewModel result = (CartIndexViewModel)controller.Index(cart, "myUrl").ViewData.Model;
            // assert
            Assert.AreEqual(cart, result.Cart);
            Assert.AreEqual("myUrl", result.ReturnUrl);
        }

        private Mock<IProductRepository> getMockProductRepository()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat1"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat2"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3" }
            });
            return mock;
        }
    }
}
