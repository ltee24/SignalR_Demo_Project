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
    public class RoomMemberController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public RoomMemberController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("/[controller]/JoinRoom")]
        public async Task<IActionResult> JoinRoom(RoomMembershipRequestVM request)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _db.Users.FirstOrDefault(u => u.Id == UserId).Name;
            var roomMember = new ChatRoomUsers
            {
                ChatRoomId = request.RoomId,
                MemberName = userName
            };
            bool isUserRoomMember = await _db.ChatRoomUsers.AnyAsync(u => u.MemberName == userName && u.ChatRoomId == request.RoomId);
            if (!isUserRoomMember)
            {
                _db.ChatRoomUsers.Add(roomMember);
                await _db.SaveChangesAsync();
                return Ok( roomMember);
            }
            return Conflict("You are already a room member");
        }

        //[HttpGet]
        //[Route("/[controller]/GetUserRooms")]
        //public async Task<IActionResult> GetUserRooms()
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var userName = _db.Users.FirstOrDefault(u => u.Id == UserId).Name;
        //    var userRooms = _db.ChatRoomUsers;
        //}
    }
}
