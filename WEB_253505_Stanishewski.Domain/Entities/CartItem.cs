using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253505_Stanishewski.Domain.Entities
{
    public class CartItem
    {
        public int Amount { get; set; }
        public Game Item { get; set; }
    }
}
