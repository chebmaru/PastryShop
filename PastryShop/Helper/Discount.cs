using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PastryShop.Helper
{
    public class Discount
    {
        public Discount()
        {

        }
        Dictionary<int, int> discountByDay = new Dictionary<int, int>
        {{1,0 }, {2,20} , {3,80}};
        public decimal GetDiscount(decimal price, DateTime sellingDate)
        {
            decimal discount = 0;
            int sellingDays = (DateTime.Now - sellingDate).Days + 1;
            if (discountByDay.ContainsKey(sellingDays))
                discount = price * discountByDay[sellingDays] / 100;
            else discount = price;
            return discount;
        }
    }
}
