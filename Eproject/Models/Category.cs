using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Eproject.Models
{
    public class Category
    {
        [Key]
        public int CatID { get; set; }
        public string? Cat_Name { get; set; } 
        public ICollection<Product> Products { get; set; }
    }
}
