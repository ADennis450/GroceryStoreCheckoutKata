using GroceryStoreCheckoutKata.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreCheckoutKata.CheckoutLogic
{
    public class ShoppingCart
    {
        private static List<Item> ShoppingCartList;
        
        private static int id = 0;

        public static void CreateShoppingCart()
        {
            if(ShoppingCartList == null)
            {
                ShoppingCartList = new List<Item>();
            }
        }

        public static void ClearShoppingCart()
        {
            ShoppingCartList.Clear();
        }

        public static List<Item> GetShoppingCart()
        {
            return ShoppingCartList;
        }

        public static void AddGroceryItem(Item item)
        {
            id += 1;
            item.Id = id;
            ShoppingCartList.Add(item);
        }
        
        public static List<Item> FindItems(object itemValue, ItemTypeEnum itemFilterType)
        {
            switch(itemFilterType)
            {
                case ItemTypeEnum.Id:
                    return ShoppingCartList.FindAll(item => item.Id == (int)itemValue);
                case ItemTypeEnum.Name:
                    return ShoppingCartList.FindAll(item => item.Name == (string)itemValue);
                case ItemTypeEnum.Price:
                    return ShoppingCartList.FindAll(item => item.Price == (double)itemValue);
                default:
                    return ShoppingCartList;
            }
        }

        public static void RemoveItemFromCart(string itemName)
        {
            if (ShoppingCartList.Count > 0)
            {
                Item itemToBeRemoved = FindItems(itemName, ItemTypeEnum.Name)[0];
                ShoppingCartList.Remove(itemToBeRemoved);
            }
        }

        public static string CalculatePrice()
        {
            double totalPrice = 0;
            foreach (Item item in ShoppingCartList)
            {
                totalPrice += item.Price;
            }

            return totalPrice.ToString("0.00");
        }
    }
}
