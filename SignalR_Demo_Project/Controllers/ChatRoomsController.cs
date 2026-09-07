using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR_Demo_Project.Data;
using SignalR_Demo_Project.Models;

namespace SignalR_Demo_Project.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    [Authorize]
    public class ChatRoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ChatRoomsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("/[controller]/GetRooms")]
        public async Task<ActionResult<IEnumerable<ChatRoom>>> GetRooms()
        {
            return await _db.ChatRooms.ToListAsync();
        }
        [HttpGet]
        [Route("/[controller]/GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _db.Users.ToList();
            return Ok(users);
        }

        [HttpGet]
        [Route("/[controller]/GetUser/{id}")]
        public async Task<IActionResult> GetUser()
        {
            return Ok();
        }

        [HttpPost]
        [Route("/[controller]/CreateRoom")]
        public async Task<IActionResult> CreateRoom(ChatRoom chatRoom)
        {
            _db.ChatRooms.Add(chatRoom);
            await _db.SaveChangesAsync();
            return Created($"/ChatRooms/GetRoom/{chatRoom.Id}", chatRoom);

        }
    }
}
