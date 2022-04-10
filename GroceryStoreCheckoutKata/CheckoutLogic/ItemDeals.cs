using GroceryStoreCheckoutKata.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GroceryStoreCheckoutKata.CheckoutLogic
{
    public class ItemDeals
    {
        private readonly static string[] AvailableDeals = { "bogo", "halfoff" };
        private static Dictionary<string, List<string>> ActiveDeals;
        
        public static void CreateActiveDeals()
        {
            if (ActiveDeals == null)
            {
                ActiveDeals = new Dictionary<string, List<string>>();

                foreach (string deal in AvailableDeals)
                {
                    ActiveDeals.Add(deal, new List<string>());
                }
            }
        }
        
        public static Dictionary<string, List<string>> ApplyItemToDeal(Deal deal)
        {
            CreateActiveDeals();
            string formattedDealName = Regex.Replace(deal.DealName, @"\s+", "").ToLower();
            List<Item> itemList = new List<Item>();

            switch (formattedDealName)
            {
                case "bogo":
                    itemList = BOGO(deal);
                    break;
                case "halfoff":
                    itemList = HalfOff(deal);
                    break;
                default:
                    break;
            }
            modifyShoppingCartItems(itemList);
            
            if(!ActiveDeals.ContainsKey(formattedDealName))
            {
                throw new Exception(formattedDealName + " is not a part of available deals");
            }
            else
            {
                AddItemToAvailableDealsList(formattedDealName, deal.ItemName);
            }
            return ActiveDeals;
        }

        public static Dictionary<string, List<string>> GetActiveDeals()
        {
            return ActiveDeals;
        }

        private static List<Item> HalfOff(Deal deal)
        {
            List<Item> modifiedOrderItems = new List<Item>();
            List<Item> Items = ShoppingCart.FindItems(deal.ItemName, ItemTypeEnum.Name);
            int dealMetCount = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                if (dealMetCount == deal.DealLimit)
                {
                    break;
                }
                dealMetCount += 1;
                Items[i].Price = Items[i].Price / 2;
                modifiedOrderItems.Add(Items[i]);
            }
            return Items;
        }

        private static List<Item> BOGO(Deal deal)
        {
            List<Item> modifiedItems = new List<Item>();
            List<Item> items = ShoppingCart.FindItems(deal.ItemName, ItemTypeEnum.Name);
            int dealMetCount = 0;

            for (int i = 0; i < items.Count; i++)
            {
                if (dealMetCount == deal.DealLimit)
                {
                    break;
                }
                dealMetCount += 1;
                if (dealMetCount > 1)
                {
                    items[i].Price = 0;
                    dealMetCount = 0;
                }
                modifiedItems.Add(items[i]);
            }
            return modifiedItems;
        }

        private static void modifyShoppingCartItems(List<Item> modifiedItems)
        {
            foreach (Item item in modifiedItems)
            {
                int index = ShoppingCart.GetShoppingCart().FindIndex(cartItem => cartItem.Id == item.Id);
                if (index != -1)
                {
                    ShoppingCart.GetShoppingCart()[index] = item;
                }
            }
        }

        private static void AddItemToAvailableDealsList(string dealName, string itemName)
        {
            if(!ActiveDeals[dealName].Contains(itemName))
            {
                ActiveDeals[dealName].Add(itemName);
            }
        }
    }
}
