using SignalR_Demo_Project.Models;

namespace SignalR_Demo_Project.VM
{
    public class ChatVM
    {
        public int MaxRoomAllowed { get; set; } = 4;
        public IList<ChatRoom> Rooms { get; set; } = new List<ChatRoom>();

        public string? UserId { get; set; }

        public bool AllowAddRoom => Rooms == null || Rooms.Count < MaxRoomAllowed;
    }
}
