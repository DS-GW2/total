using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using GW2Miner.Engine;
using GW2Miner.Domain;

namespace total
{
    class Program
    {
        static TradeWorker trader = new TradeWorker();

        static int totalBought(bool past)
        {
            int total = 0;
            List<Item> itemBoughtList = trader.get_my_buys(true, past).Result;
            foreach (Item item in itemBoughtList)
            {
                total = total + item.UnitPrice * item.Quantity;
            }

            return total;
        }

        static int totalSold(bool past, bool now)
        {
            int total = 0;
            List<Item> itemSoldList = trader.get_my_sells(true, past).Result;
            foreach (Item item in itemSoldList)
            {
                if (now)
                {
                    List<Item> currentItems = trader.get_items(item.Id).Result;
                    total = total + (int)((currentItems[0].MinSaleUnitPrice - 1) * item.Quantity * 0.85);
                }
                else
                {
                    total = total + item.UnitPrice * item.Quantity;
                }
            }

            return total;
        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Total in Pending Sell List = {0}", totalSold(false, false));
                Console.WriteLine("Total in Pending Sell List, if liquidate now = {0}", totalSold(false, true));
                Console.WriteLine("Total in Pending Buy List = {0}", totalBought(false));

                int totalItemsBought = totalBought(true);
                int totalItemsSold = totalSold(true, false);
                Console.WriteLine("Total Bought = {0}", totalItemsBought);
                Console.WriteLine("Total Sold = {0}", totalItemsSold);
                Console.WriteLine("Profit = {0}", totalItemsSold - totalItemsBought);
            }
            catch (Exception e)
            {
                Console.WriteLine(ExceptionHelper.FlattenException(e));
            }

            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }
    }
}
