using Ecom.Data;
using Ecom.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ecom.Models
{
    public class Validators
    {
    }
    public class MaxFileSize : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var product = validationContext.ObjectInstance as ProductModel;
            if (product.Image == null)
            {
                return new ValidationResult("Choose file");
            }
            var file = product.Image as HttpPostedFileBase;
            var maxSize = Math.Pow(2, 20);
            if (file.ContentLength > maxSize)
            {
                return new ValidationResult("File size shouldn't exceed 2MB");
            }
            return ValidationResult.Success;
        }
    }
}