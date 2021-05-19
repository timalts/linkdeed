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
    public class EmployerDescriptionController : ControllerBase
    {
        private readonly Context _context;
        public EmployerDescriptionController(Context context)
        {
            _context = context;
        }

        // GET: DescriptionController
        [HttpGet("CurentEmployerDescription")]
        public async Task<ActionResult<EmployerDescription>> GetDescription()
        {
            //Finding who is logged in
            int logged_id = int.Parse(User.Identity.Name);

            var desc = _context.EmployerDescription.ToList().Find(x => x.UserId == logged_id);

            if (desc == null)
                return BadRequest(new { message = "Your are not loged-in" });

            return desc;
        }

        // GET: JobsController
        [Authorize(Roles = AccessLevel.Admin)]
        [HttpGet("id")]
        public async Task<ActionResult<EmployerDescription>> GetDescription_ById(int id)
        {
            var desc = _context.EmployerDescription.ToList().Find(x => x.Id == id);

            if (desc == null)
                return BadRequest(new { message = "Description not found." });

            return desc;
        }

        // GET: JobsController
        [Authorize(Roles = AccessLevel.Admin)]
        [HttpGet("UserId")]
        public async Task<ActionResult<EmployerDescription>> GetDescription_ByUserId(int UserId)
        {
            var desc = _context.EmployerDescription.ToList().Find(x => x.UserId == UserId);

            if (desc == null)
                return BadRequest(new { message = "Description not found." });

            return desc;
        }

        [Authorize(Roles = AccessLevel.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployerDescription>> Delete_Description(int id)
        {
            var desc = _context.EmployerDescription.Find(id);


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
        public async Task<ActionResult> Update_Desc_ByID(int id, EmployerDescriptionDTO desc)
        {
            if (!DescExists(id))
            {
                return BadRequest(new { message = "Description not found." });
            }
            else
            {
                var _desc = _context.EmployerDescription.SingleOrDefault(x => x.Id == id);
                _desc.Description = desc.Description;
                _desc.IsPrenium = desc.IsPrenium;


                await _context.SaveChangesAsync();
                return Ok("User description has been updated!"); 
            }
        }

        [Authorize(Roles = AccessLevel.Admin)]
        [HttpPut("UserId")]
        public async Task<ActionResult> Update_Desc_ByUserID(int UserId, EmployerDescriptionDTO desc)
        {
            if (!_context.EmployerDescription.Any(x => x.UserId == UserId))
            {
                return BadRequest(new { message = "Description not found." });
            }
            else
            {
                var _desc = _context.EmployerDescription.SingleOrDefault(x => x.UserId == UserId);
                _desc.Description = desc.Description;
                _desc.IsPrenium = desc.IsPrenium;


                await _context.SaveChangesAsync();
                return Ok("User description has been updated!");
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update_Desc(EmployerDescriptionDTO desc)
        {
            //Finding who is logged in
            int logged_id = int.Parse(User.Identity.Name);

            
            var _desc = _context.EmployerDescription.SingleOrDefault(x => x.UserId == logged_id);

            if(_desc == null)
                return BadRequest(new { message = "There is no description linked to your account" });
            
            _desc.Description = desc.Description;
            _desc.IsPrenium = desc.IsPrenium;


            await _context.SaveChangesAsync();
            return Ok("User description has been updated!");

        }

        private bool DescExists(int id)
        {
            return _context.EmployerDescription.Any(x => x.Id == id);
        }
    }
}
