using Linkdeed.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Linkdeed.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Linkdeed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly Context _context;
        public MessageController(Context context)
        {
            _context = context;
        }
        // GET: api/<MessageController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessage()
        {
            var messages = from message in _context.Message
                           select new Message
                           {
                               Id = message.Id,
                               ReceiverId = message.ReceiverId,
                               SenderId = message.SenderId,
                               Body = message.Body
                           };
            return await messages.ToListAsync();
        }

        // GET api/<MessageController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage_ById(int id)
        {
            return _context.Message.ToList().Find(x => x.Id == id);
        }

        // GET api/<MessageController>/Sender/6
        [HttpGet("{SenderId}")]
        public async Task<ActionResult<Message>> GetMessage_BySenderId(int SenderId)
        {
            return _context.Message.ToList().Find(x => x.SenderId == SenderId);
        }

        // GET api/<MessageController>/Receiver/7
        [HttpGet("{ReceiverId}")]
        public async Task<ActionResult<Message>> GetMessage_ByReceiverId(int ReceiverId)
        {
            return _context.Message.ToList().Find(x => x.ReceiverId == ReceiverId);
        }

        [HttpPost]
        public async Task<ActionResult<Message>> Add_Message (Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var messages = new Message()
            {
                Id = message.Id,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Body = message.Body
            };

            await _context.Message.AddAsync(messages);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage_ById", new { id = messages.Id }, message);


        }
    }
}
