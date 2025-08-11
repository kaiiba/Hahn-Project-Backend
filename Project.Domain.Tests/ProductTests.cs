using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.Domain.Entities;

namespace Project.Domain.Tests
{
    [TestClass]
    public class ProductTests
    {
        [TestMethod]
        public void Create_ValidProduct_ShouldCreateSuccessfully()
        {
            var name = "Test Product";
            var price = 99.99m;
            var stock = 10;

            var product = Product.Create(name, price, stock);

            Assert.AreEqual(name, product.Name);
            Assert.AreEqual(price, product.Price);
            Assert.AreEqual(stock, product.Stock);
            Assert.IsTrue(product.CreatedAt <= DateTime.UtcNow);
            Assert.AreEqual(0, product.Id);
        }

        [TestMethod]
        public void Create_EmptyName_ShouldThrowArgumentException()
        {
            var name = "";
            var price = 99.99m;
            var stock = 10;

            var exception = Assert.ThrowsException<ArgumentException>(() => Product.Create(name, price, stock));
            Assert.AreEqual("The product name must be filled up.", exception.Message);
        }

        [TestMethod]
        public void Create_WhitespaceName_ShouldThrowArgumentException()
        {
            var name = "   ";
            var price = 99.99m;
            var stock = 10;

            var exception = Assert.ThrowsException<ArgumentException>(() => Product.Create(name, price, stock));
            Assert.AreEqual("The product name must be filled up.", exception.Message);
        }

        [TestMethod]
        public void Create_NullName_ShouldThrowArgumentException()
        {
            string name = null;
            var price = 99.99m;
            var stock = 10;

            var exception = Assert.ThrowsException<ArgumentException>(() => Product.Create(name, price, stock));
            Assert.AreEqual("The product name must be filled up.", exception.Message);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-99.99)]
        public void Create_InvalidPrice_ShouldThrowArgumentException(double priceValue)
        {
            var name = "Test Product";
            var price = (decimal)priceValue;
            var stock = 10;

            var exception = Assert.ThrowsException<ArgumentException>(() => Product.Create(name, price, stock));
            Assert.AreEqual("The product price must be positive.", exception.Message);
        }

        [TestMethod]
        public void DecreaseStock_ValidQuantity_ShouldDecreaseStock()
        {
            var product = Product.Create("Test Product", 99.99m, 10);
            var decreaseBy = 5;

            product.DecreaseStock(decreaseBy);

            Assert.AreEqual(5, product.Stock);
        }

        [TestMethod]
        public void DecreaseStock_QuantityGreaterThanStock_ShouldThrowInvalidOperationException()
        {
            var product = Product.Create("Test Product", 99.99m, 10);
            var decreaseBy = 15;

            var exception = Assert.ThrowsException<InvalidOperationException>(() => product.DecreaseStock(decreaseBy));
            Assert.AreEqual("The product quantity must be less or equal the quantity in stock.", exception.Message);
        }

        [TestMethod]
        public void SetId_ValidId_ShouldSetIdCorrectly()
        {
            var product = Product.Create("Test Product", 99.99m, 10);
            var expectedId = 123;

            product.SetId(expectedId);

            Assert.AreEqual(expectedId, product.Id);
        }
    }
}
