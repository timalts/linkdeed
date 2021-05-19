using AutoMapper;
using Linkdeed.Data;
using Linkdeed.DTO;
using Linkdeed.Helper;
using Linkdeed.Models;
using Linkdeed.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linkdeed.Controllers
{
    [Authorize(Roles = AccessLevel.Admin)]
    [ApiController]
    [Route("[controller]")]
    public class AdminController : Controller
    {

        private IUserService _userService;
        private IMapper _mapper;
        public IConfiguration Configuration;
        private readonly Context _context;


        public AdminController(
            Context context,
            IUserService userService,
            IMapper mapper,
            IConfiguration configuration)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
            Configuration = configuration;
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterAdminModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);
            user.AccesLevel = "Admin";

            try
            {
                // create user
                _userService.AdminCreate(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = AccessLevel.Admin)]
        [HttpPost("accesslevel/{id}")]
        public IActionResult ChangeAccess(int id, UpdateAccessLevel model)
        {
            // value except the allowed values which are: Admin, Employer, User

            if(model.AccessLevel != "Admin" && model.AccessLevel != "Employer" && model.AccessLevel != "User")
                return BadRequest(new { message = "Invalid AccesLevel, please try Admin, Employer or User" });

            _context.User.Find(id).AccesLevel = model.AccessLevel;
            _context.SaveChanges();
            return Ok("User Access Level has been updated!");
        }
    }
}
