using Linkdeed.Data;
using Linkdeed.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Linkdeed.DTO;

namespace Linkdeed.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class JobOffersOfferController : ControllerBase
    {
        private readonly Context _context;
        public JobOffersOfferController(Context context)
        {
            _context = context;
        }
        // GET: JobOffersController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobOffer>>> GetJobOffers()
        {
            var jobsoffer = from JobOffer in _context.JobOffer
                       select new JobOffer
                       {
                           Id = JobOffer.Id,
                           JobId = JobOffer.JobId,
                           OfferStatus = JobOffer.OfferStatus,
                       };

            return await jobsoffer.ToListAsync();
        }

        // GET: JobOffersController/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobOffer>> GetJobOffers_ById(int id)
        { 
            return _context.JobOffer.ToList().Find(x => x.Id == id);
        }

        // GET: JobOffersController/Create 
        [Authorize(Roles = AccessLevel.Employer + "," + AccessLevel.Admin)]
        [HttpPost]
        public async Task<ActionResult<JobOffer>> Add_JobOffers(JobOffer JobOffer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var JobOffers = new JobOffer()
            {
                Id = JobOffer.Id,
                JobId = JobOffer.JobId,
                OfferStatus = JobOffer.OfferStatus,
            };

            await _context.JobOffer.AddAsync(JobOffers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobOffers_ById", new { id = JobOffers.Id }, JobOffer);
        }


        [Authorize(Roles = AccessLevel.Employer + "," + AccessLevel.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<JobOffer>> Delete_JobOffer(int id)
        {
            //Finding who is logged in
            int logged_in_user = int.Parse(User.Identity.Name);
            
            //Rejecting access if the logged in user is not same as the user updating information
            if (User.IsInRole("Employer") && logged_in_user != _context.Job.Find(_context.JobOffer.Find(id).JobId).User_Id)
            {
                return BadRequest(new { message = "Access Denied" });
            }
            var JobOffers = _context.JobOffer.Find(id);


            if (JobOffers == null)
            {
                return NotFound();
            }
            else
            {
                _context.Remove(JobOffers);

                await _context.SaveChangesAsync();
                return JobOffers;
            }
        }

        [Authorize(Roles = AccessLevel.Employer + "," + AccessLevel.Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update_Books(int id, JobOffer JobOffer)
        {
            //Finding who is logged in
            int logged_in_user = int.Parse(User.Identity.Name);
            
            //Rejecting access if the logged in user is not same as the user updating information
            if (User.IsInRole("Employer") && logged_in_user != _context.Job.Find(_context.JobOffer.Find(id).JobId).User_Id)
            {
                return BadRequest(new { message = "Access Denied" });
            }
            if (id != JobOffer.Id || !BookExists(id))
            {
                return BadRequest();
            }
            else
            {
                var JobOffers = _context.JobOffer.SingleOrDefault(x => x.Id == id);
                JobOffers.Id = JobOffer.Id;
                JobOffers.JobId = JobOffer.JobId;
                JobOffers.OfferStatus = JobOffer.OfferStatus;


                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

        private bool BookExists(int id)
        {
            return _context.JobOffer.Any(x => x.Id == id);
        }

    }
}
