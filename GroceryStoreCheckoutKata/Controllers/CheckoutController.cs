using GroceryStoreCheckoutKata.CheckoutLogic;
using GroceryStoreCheckoutKata.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreCheckoutKata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {

        [HttpPost("CreateOrderItem")]
        public ActionResult CreateOrderItem(Item item)
        {
            ShoppingCart.AddGroceryItem(item);
            List<Item> groceryItems = ShoppingCart.GetShoppingCart();
            return Content(JsonConvert.SerializeObject(groceryItems));
        }

        [HttpPost("RemoveOrderItem")]
        public ActionResult RemoveOrderItem(string itemName)
        {
            //TODO: Refactor to remove item based on name
            ShoppingCart.RemoveItemFromCart(itemName);
            List<Item> groceryItems = ShoppingCart.GetShoppingCart();
            return Content(JsonConvert.SerializeObject(groceryItems));
        }

        [HttpPost("ApplyDealToOrderItem")]
        public ActionResult ApplyDealToOrderItem(Deal deal)
        {
            ItemDeals.ApplyItemToDeal(deal);
            return Content(JsonConvert.SerializeObject(ShoppingCart.GetShoppingCart()));
        }

        [HttpPost("Checkout")]
        public ActionResult Checkout()
        {
            string totalPrice = ShoppingCart.CalculatePrice();
            return Content($"Total checkout price is ${totalPrice}");
        }

        [HttpGet]
        public void Default()
        {
            ShoppingCart.CreateShoppingCart();
        }
    }
}