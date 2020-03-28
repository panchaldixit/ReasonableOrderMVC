using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReasonableOrderMVC.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Required]
        [Range(1, 150, ErrorMessage = "Please enter a correct quantity")]
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Total { get; set; }
        public int OrderID { get; set; }
        public List<Product> AvailableProduct { get; set; }

    }

    public class Sale 
    { 
        [Key]
        public int Id { get; set; }
        public string Total_Order_Value { get; set; }
    }

    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int OrderId { get; set; }

    }

    public class ProductList {
        public Order _order { get; set; }
        public List<Product> _products { get; set; }
    }
}
