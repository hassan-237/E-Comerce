
using Eproject.Enums;

namespace Eproject.ViewModels
{
    public class RegisterVM
    {

        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public UserRole Role { get; set; }
        public  string? ErrorMessage { get; set; }




    }
}
