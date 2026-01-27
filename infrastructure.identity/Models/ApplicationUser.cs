using Microsoft.AspNetCore.Identity;

namespace infrastructure.identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
    }
}
