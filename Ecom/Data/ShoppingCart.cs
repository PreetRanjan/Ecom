//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ecom.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class ShoppingCart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public Nullable<int> Quantity { get; set; }
    
        public virtual AppUser AppUser { get; set; }
        public virtual Product Product { get; set; }
    }
}
