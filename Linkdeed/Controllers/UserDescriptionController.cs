using Linkdeed.Data;
using Linkdeed.DTO;
using Linkdeed.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linkdeed.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserDescriptionController : ControllerBase
    {
        private readonly Context _context;
        public UserDescriptionController(Context context)
        {
            _context = context;
        }

        // GET: DescriptionController
        [HttpGet("CurentUserDescription")]
        public async Task<ActionResult<UserDescription>> GetDescription()
        {
            //Finding who is logged in
            int logged_id = int.Parse(User.Identity.Name);

            var desc = _context.UserDescription.ToList().Find(x => x.UserId == logged_id);

            if (desc == null)
                return BadRequest(new { message = "Your are not loged-in" });

            return desc;
        }

        // GET: JobsController
        [Authorize(Roles = AccessLevel.Admin)]
        [HttpGet("id")]
        public async Task<ActionResult<UserDescription>> GetDescription_ById(int id)
        {
            var desc = _context.UserDescription.ToList().Find(x => x.Id == id);

            if (desc == null)
                return BadRequest(new { message = "Description not found." });

            return desc;
        }

        // GET: JobsController
        [Authorize(Roles = AccessLevel.Admin)]
        [HttpGet("UserId")]
        public async Task<ActionResult<UserDescription>> GetDescription_ByUserId(int UserId)
        {
            var desc = _context.UserDescription.ToList().Find(x => x.UserId == UserId);

            if (desc == null)
                return BadRequest(new { message = "Description not found." });

            return desc;
        }

        [Authorize(Roles = AccessLevel.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDescription>> Delete_Description(int id)
        {
            var desc = _context.UserDescription.Find(id);


            if (desc == null)
            {
                return NotFound();
            }
            else
            {
                _context.Remove(desc);

                await _context.SaveChangesAsync();
                return desc;
            }
        }


        [Authorize(Roles = AccessLevel.Admin)]
        [HttpPut("id")]
        public async Task<ActionResult> Update_Desc_ByID(int id, UserDescriptionDTO desc)
        {
            if (!DescExists(id))
            {
                return BadRequest(new { message = "Description not found." });
            }
            else
            {
                var _desc = _context.UserDescription.SingleOrDefault(x => x.Id == id);
                _desc.Description = desc.Description;


                await _context.SaveChangesAsync();
                return Ok("User description has been updated!"); 
            }
        }

        [Authorize(Roles = AccessLevel.Admin)]
        [HttpPut("UserId")]
        public async Task<ActionResult> Update_Desc_ByUserID(int UserId, UserDescriptionDTO desc)
        {
            if (!_context.UserDescription.Any(x => x.UserId == UserId))
            {
                return BadRequest(new { message = "Description not found." });
            }
            else
            {
                var _desc = _context.UserDescription.SingleOrDefault(x => x.UserId == UserId);
                _desc.Description = desc.Description;


                await _context.SaveChangesAsync();
                return Ok("User description has been updated!");
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update_Desc(UserDescriptionDTO desc)
        {
            //Finding who is logged in
            int logged_id = int.Parse(User.Identity.Name);

            
            var _desc = _context.UserDescription.SingleOrDefault(x => x.UserId == logged_id);

            if(_desc == null)
                return BadRequest(new { message = "There is no description linked to your account" });
            
            _desc.Description = desc.Description;


            await _context.SaveChangesAsync();
            return Ok("User description has been updated!");

        }

        private bool DescExists(int id)
        {
            return _context.UserDescription.Any(x => x.Id == id);
        }
    }
}
