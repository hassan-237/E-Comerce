using Eproject.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Eproject.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public UserRole Role { get; set; }
    }
}
