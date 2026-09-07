namespace SignalR_Demo_Project.Models
{
    public class ChatRoomUsers
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }

        public string MemberName { get; set; }
        
    }
}
