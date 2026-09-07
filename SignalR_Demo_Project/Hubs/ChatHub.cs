using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR_Demo_Project.Data;
using System.Security.Claims;

namespace SignalR_Demo_Project.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _db;
        public ChatHub(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task SendMessageToAll(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendAddRoomMessage(int roomId,string roomName)
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _db.Users.FirstOrDefault(u => u.Id == UserId).UserName;

            await Clients.All.SendAsync("RecieveCreateRoomMessage",  roomName, userName);
        }

        public async Task SendJoinRoomMessage(int roomId,string memberName)
        {
            var roomName = _db.ChatRooms.FirstOrDefault(u => u.Id == roomId).Name;
            await Clients.All.SendAsync("RecieveJoinRoomMessage", roomName, memberName);
        }

        public async Task SendPrivateMessage(string receiverId,string receiverName,string message)
        {
            var senderId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var senderName = _db.Users.FirstOrDefault(u=>u.Id == senderId).Name;
            var users = new string[] { senderId, receiverId };
            await Clients.Users(users).SendAsync("ReceievePrivateMessage", senderName, receiverName, message);
        }

        public async Task SendRoomMessage(int roomId,string message)
        {
            var senderId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var senderName = _db.Users.FirstOrDefault(u => u.Id == senderId).Name;
            var roomMembers = await _db.ChatRoomUsers.Where(x=>x.ChatRoomId == roomId).Select(x => x.UserId).ToListAsync();
            await Clients.Users(roomMembers).SendAsync("ReceiveRoomMessage", senderName,message);
        }
    }
}
