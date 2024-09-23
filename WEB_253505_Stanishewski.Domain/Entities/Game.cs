using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253505_Stanishewski.Domain.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Category? Category { get; set; }

        public string? Image { get; set; }
        public decimal Price { get; set; }

        public int? CategoryId { get; set; }

    }
}
