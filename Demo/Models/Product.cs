using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Demo.Models
{
    public class Product
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "You need enter product name")]
        [StringLength(255, ErrorMessage = "You can only enter up to 255 characters")]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "You need to enter status as number 0 or number 1")]
        [Range(0, 1, ErrorMessage = "You can only enter number 0 or number 1")]
        public int Status { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Created_at { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Updated_at { get; set; }
        [Required(ErrorMessage = "You need enter product price")]
        [Range(1, 999999, ErrorMessage = "You need to enter the price between 1 and 999,999")]
        [Display(Name = "Product Price")]
        public double Price { get; set; }
        public string Direction { get; set; }
    }
}