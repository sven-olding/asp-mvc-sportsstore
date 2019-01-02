using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            });
            ProductController controller = new ProductController(mock.Object)
            {
                PageSize = 3
            };
            // act
            var result = (ProductsListViewModel)controller.List(null, 2).Model;
            // assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // arrange
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            // act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);
            // assert
            Assert.AreEqual(@"<a class=""btn-secondary btn"" href=""Page1"">1</a>"
                          + @"<a class=""btn-primary btn"" href=""Page2"">2</a>"
                          + @"<a class=""btn-secondary btn"" href=""Page3"">3</a>"
                            , result.ToString());

        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // arrange 
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            });
            ProductController controller = new ProductController(mock.Object)
            {
                PageSize = 3
            };
            // act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;
            // assert
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            // arrange 
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat1"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat2"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3" }
            });
            ProductController controller = new ProductController(mock.Object)
            {
                PageSize = 2
            };
            // act
            ProductsListViewModel result = (ProductsListViewModel)controller.List("Cat2", 1).Model;
            // assert 
            Assert.AreEqual(result.CurrentCategory, "Cat2");
            Assert.AreEqual(result.Products.ToArray().Length, 2);
            Assert.AreEqual(result.Products.ElementAt(0).Name, "P3");
            Assert.AreEqual(result.Products.ElementAt(0).Category, "Cat2");
            Assert.AreEqual(result.Products.ElementAt(1).Name, "P4");
            Assert.AreEqual(result.Products.ElementAt(1).Category, "Cat2");
        }
    }
}
