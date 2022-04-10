using Microsoft.VisualStudio.TestTools.UnitTesting;
using GroceryStoreCheckoutKata.CheckoutLogic;
using System;
using System.Collections.Generic;
using System.Text;
using GroceryStoreCheckoutKata.Models;
using Newtonsoft.Json;

namespace GroceryStoreCheckoutKata.CheckoutLogic.Tests
{
    [TestClass()]
    public class GroceryCartTests
    {
        [TestMethod]
        public void AddGroceryItemTest_ShouldReturnOneItem()
        {
            ShoppingCart.CreateShoppingCart();
            string itemList = "{'Name': 'apple', 'Price': 0.99, 'Weight': 0.15}";
            Item orderItem = JsonConvert.DeserializeObject<Item>(itemList);
            ShoppingCart.AddGroceryItem(orderItem);
            List<Item> items = ShoppingCart.GetShoppingCart();
            int orderItemCount = items.Count;
            ShoppingCart.ClearShoppingCart();
            Assert.AreEqual(orderItemCount, 1);
            Console.WriteLine($"There are {orderItemCount} item in the orderItem array");
        }

        [TestMethod]
        public void RemoveGroceryItemTest_ShouldHaveOneLessItemInGroceryCart()
        {
            ShoppingCart.CreateShoppingCart();
            string itemList = "{'Name': 'apple', 'Price': 0.99, 'Weight': 0.15}";
            Item orderItem = JsonConvert.DeserializeObject<Item>(itemList);
            ShoppingCart.AddGroceryItem(orderItem);
            int intialOrderItemCount = ShoppingCart.GetShoppingCart().Count;
            ShoppingCart.RemoveItemFromCart("apple");
            int afterRemovalOrderItemCount = ShoppingCart.GetShoppingCart().Count;
            ShoppingCart.ClearShoppingCart();
            Assert.AreEqual(intialOrderItemCount, (1 + afterRemovalOrderItemCount));
        }

        [TestMethod]
        public void CalculatePriceWithBogoDealTest_ShouldApplyBogoDealtoOrderItems()
        {
            ShoppingCart.CreateShoppingCart();
            ItemDeals.CreateActiveDeals();
            string itemList = "[{'Name': 'apple', 'Price': 0.99, 'Weight': 0.15}," +
                     " {'Name': 'apple', 'Price': 0.99, 'Weight': 0.15}," +
                     " {'Name': 'apple', 'Price': 0.99, 'Weight': 0.15}," +
                     " {'Name': 'apple', 'Price': 0.99, 'Weight': 0.15}]";

            List<Item> orderItems = JsonConvert.DeserializeObject<List<Item>>(itemList);
            foreach (Item item in orderItems)
            {
                ShoppingCart.AddGroceryItem(item);
            }
            Deal deal = new Deal();
            deal.DealLimit = 4;
            deal.DealName = "bogo";
            deal.ItemName = "apple";
            Dictionary<string, List<string>> items = ItemDeals.ApplyItemToDeal(deal);
            string price = ShoppingCart.CalculatePrice();
            ShoppingCart.ClearShoppingCart();
            Assert.AreEqual(price, "1.98");
        }

        [TestMethod]
        public void CalculatePriceWithHalfOffDealTest_ShouldApplyHalfOffToPrice()
        {
            ShoppingCart.CreateShoppingCart();
            string itemList = "[{'Name': 'apple', 'Price': 0.99, 'Weight': 0.15}," +
                     " {'Name': 'apple', 'Price': 0.99, 'Weight': 0.15}," +
                     " {'Name': 'apple', 'Price': 0.99, 'Weight': 0.15}," +
                     " {'Name': 'apple', 'Price': 0.99, 'Weight': 0.15}]";

            List<Item> orderItems = JsonConvert.DeserializeObject<List<Item>>(itemList);
            foreach (Item item in orderItems)
            {
                ShoppingCart.AddGroceryItem(item);
            }
            Deal deal = new Deal();
            deal.DealLimit = 4;
            deal.DealName = "half off";
            deal.ItemName = "apple";
            Dictionary<string, List<string>> items = ItemDeals.ApplyItemToDeal(deal);
            string price = ShoppingCart.CalculatePrice();
            ShoppingCart.ClearShoppingCart();
            Assert.AreEqual(price, "1.98");
        }

        public void CalculatePriceWithHalfOffAndBogoDealTest()
        {
            ShoppingCart.CreateShoppingCart();
            string itemList = "[{'Name': 'apple', 'Price': 0.99, 'Weight': 0.15}," +
                     " {'Name': 'apple', 'Price': 0.99, 'Weight': 0.15}," +
                     " {'Name': 'banana', 'Price': 2.99, 'Weight': 0.15}," +
                     " {'Name': 'banana', 'Price': 2.99, 'Weight': 0.15}]";

            List<Item> orderItems = JsonConvert.DeserializeObject<List<Item>>(itemList);
            foreach (Item item in orderItems)
            {
                ShoppingCart.AddGroceryItem(item);
            }
            Deal appleDeal = new Deal();
            appleDeal.DealLimit = 4;
            appleDeal.DealName = "bogo";
            appleDeal.DealName = "apple";

            Deal bananaDeal = new Deal();
            bananaDeal.DealLimit = 4;
            bananaDeal.DealName = "half off";
            bananaDeal.ItemName = "banana";
            Dictionary<string, List<string>> items = ItemDeals.ApplyItemToDeal(appleDeal);
            string price = ShoppingCart.CalculatePrice();
            ShoppingCart.ClearShoppingCart();
            Assert.AreEqual(price, "3.98");
        }
    }
}