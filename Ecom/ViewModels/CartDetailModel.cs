using Ecom.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecom.ViewModels
{
    public class CartDetailModel
    {
        public List<ShoppingCart> CartItems { get; set; }

        public CartDetailModel(List<ShoppingCart> cartItems)
        {
            CartItems = cartItems;
        }
        public decimal TotalPrice 
        {
            get
            {
                decimal? sum = 0;
                foreach (var item in CartItems)
                {
                    sum += item.Quantity * item.Product.Price;
                }
                return (decimal)sum;
            }
        }
    }
}