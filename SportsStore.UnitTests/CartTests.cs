using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;

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
    }
}
