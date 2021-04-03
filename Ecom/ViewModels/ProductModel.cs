using Ecom.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ecom.ViewModels
{
    public class ProductModel
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MaxFileSize]
        public HttpPostedFileBase Image { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}