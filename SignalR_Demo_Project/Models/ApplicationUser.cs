using Microsoft.AspNetCore.Identity;

namespace SignalR_Demo_Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
