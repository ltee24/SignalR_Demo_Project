using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignalR_Demo_Project.Models
{
    public class ChatMessages
    {
        public int Id { get; set; } 
        
        public int RoomId { get; set; }

        [ForeignKey(nameof(RoomId))]
        public ChatRoom ChatRoom { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
