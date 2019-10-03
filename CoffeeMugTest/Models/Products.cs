using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace CoffeeMugTest.Models
{
    public partial class Products
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Decimal Price { get; set; }
    }
}
