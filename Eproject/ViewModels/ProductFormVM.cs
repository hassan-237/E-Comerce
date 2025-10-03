using Eproject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eproject.ViewModels
{
    public class ProductFormVM
    {
        public Product Product { get; set; }
        public SelectList Categories { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ErrorMessage { get; set; }

    }
}
