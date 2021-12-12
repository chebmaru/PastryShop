using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PastryShop.Models
{
    public class Ingredient 
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public string UO_Measure { get; set; }
        public int ProductID { get; set; }
    }
}
