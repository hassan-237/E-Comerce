using System.ComponentModel.DataAnnotations;

namespace Eproject.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }


        public string? Name { get; set; }

        public string? description { get; set; }

        public int price { get; set; }

        public int qty { get; set; }

        public int CatID { get; set; }

        public Category? Category { get; set; }

        public string? ImageName { get; set; }
    }
}
