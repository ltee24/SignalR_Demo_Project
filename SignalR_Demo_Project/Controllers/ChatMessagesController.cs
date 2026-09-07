using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR_Demo_Project.Data;
using SignalR_Demo_Project.Models;
using SignalR_Demo_Project.VM;
using System.Security.Claims;

namespace SignalR_Demo_Project.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ChatMessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ChatMessagesController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("/[controller]/GetPrivateMessages")]
        public async Task<ActionResult<IEnumerable<ChatMessageVM>>> GetDirectMessages(string recieverId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var senderName = _db.Users.FirstOrDefault(u => u.Id == userId).Name;
            var receiverName = _db.Users.FirstOrDefault(u => u.Id == recieverId).Name;
            var chatMessages = await _db.ChatMessages.Where(m => (m.Sender == senderName && m.Reciever == receiverName) || (m.Reciever == senderName && m.Sender == receiverName)).Select(x => new ChatMessageVM()
            {
                Sender = x.Sender,
                Reciever = x.Reciever,
                Message = x.Message,
            }).ToListAsync();
            return Ok(chatMessages);
        }

        [HttpGet]
        [Route("/[controller]/GetRoomMessages")]
        public async Task<ActionResult<IEnumerable<ChatMessageVM>>> GetRoomMessages(int roomId)
        {
            var chatMessages = await _db.ChatMessages.Where(m => m.RoomId == roomId).Select(x => new ChatMessageVM()
            {
                Sender = x.Sender,
                Reciever = x.Reciever,
                RoomId = x.RoomId,
                Message = x.Message,
            }).ToListAsync();
            return Ok(chatMessages);
        }

        [HttpPost]
        [Route("/[controller]/SaveChatMessages")]
        public async Task<IActionResult> SaveChatMessages(ChatMessageRequestVM chatMessageRequestVm)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var senderName = _db.Users.FirstOrDefault(u => u.Id == UserId).Name;
            var receiverName = _db.Users.FirstOrDefault(u => u.Id == chatMessageRequestVm.RecieverId).Name;
            var chatMessage = new ChatMessages();
            if (chatMessageRequestVm.RoomId != null)
            {
                chatMessage = new ChatMessages
                {
                    RoomId = chatMessageRequestVm.RoomId,
                    Sender = senderName,
                    Message = chatMessageRequestVm.Message
                };
            }
            else
            {
                chatMessage = new ChatMessages
                {
                    Reciever = receiverName,
                    Sender = senderName,
                    Message = chatMessageRequestVm.Message
                };
            }
            await _db.AddAsync(chatMessage);
            await _db.SaveChangesAsync();
            return Ok(new { success = true });
        }
    }
}
