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
    public class JobsController : ControllerBase
    {
        private readonly Context _context;
        public JobsController(Context context)
        {
            _context = context;
        }
        // GET: JobsController
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        {
            var jobs = from job in _context.Job
                       select new Job
                       {
                           Id = job.Id,
                           User_Id = job.User_Id,
                           JobName = job.JobName,
                           JobPayment = job.JobPayment,
                           JobDescription = job.JobDescription,
                           JobStatus = job.JobStatus,
                           EmployeeRate = job.EmployeeRate,
                       };

            return await jobs.ToListAsync();
        }

        [HttpGet("GetJobs_ByPrenium")]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs_ByPrenium()
        {
            List<Job> _jobs = await _context.Job.ToListAsync();

            List<Job> classic_job = new List<Job>();
            List<Job> jobs = new List<Job>();

            foreach (var job in _jobs)
            {
                var userDesc = _context.EmployerDescription.ToList().Find(x => x.UserId == job.User_Id);
                if(userDesc != null)
                {
                    if (userDesc.IsPremium == 0)
                    {
                        classic_job.Add(job);
                    }
                    else
                    {
                        jobs.Add(job);
                    }
                }
            }

            foreach(var job in classic_job)
            {
                jobs.Add(job);
            }

            return jobs;
        }

        // GET: JobsController/Details/5
        [HttpGet("id")]
        public async Task<ActionResult<Job>> GetJobs_ById(int id)
        {
            return _context.Job.ToList().Find(x => x.Id == id);
        }

        // GET: JobsController/Create
        [Authorize(Roles = AccessLevel.Employer + "," + AccessLevel.Admin)]
        [HttpPost]
        public async Task<ActionResult<Job>> Add_Jobs(Job job)

        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var jobs = new Job()
            {
                Id = job.Id,
                User_Id = job.User_Id,
                JobName = job.JobName,
                JobPayment = job.JobPayment,
                JobDescription = job.JobDescription,
                JobStatus = job.JobStatus,
                EmployeeRate = job.EmployeeRate
            };
            
            await _context.Job.AddAsync(jobs);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobs_ById", new { id = jobs.Id }, job);
        }

        [Authorize(Roles = AccessLevel.Employer + "," + AccessLevel.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Job>> Delete_Job(int id)
        { 
            //Finding who is logged in
            int logged_in_user = int.Parse(User.Identity.Name);
            
            //Rejecting access if the logged in user is not same as the user updating information
            if (User.IsInRole("Employer") && logged_in_user != _context.Job.Find(id).User_Id)
            {
                return BadRequest(new { message = "Access Denied" });
            }

            var jobs = _context.Job.Find(id);


            if (jobs == null)
            {
                return NotFound();
            }
            else
            {
                _context.Remove(jobs);

                await _context.SaveChangesAsync();
                return jobs;
            }
        }

        [Authorize(Roles = AccessLevel.Employer + "," + AccessLevel.Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update_Books(int id, Job job)
        { 
            //Finding who is logged in
            int logged_in_user = int.Parse(User.Identity.Name);
            
            //Rejecting access if the logged in user is not same as the user updating information
            if (User.IsInRole("Employer") && logged_in_user != _context.Job.Find(id).User_Id)
            {
                return BadRequest(new { message = "Access Denied" });
            }

            if (id != job.Id || !BookExists(id))
            {
                return BadRequest();
            }
            else
            {
                var jobs = _context.Job.SingleOrDefault(x => x.Id == id);
                jobs.Id = job.Id;
                jobs.User_Id = job.User_Id;
                jobs.JobPayment = job.JobPayment;
                jobs.JobDescription = job.JobDescription;
                jobs.JobStatus = job.JobStatus;
                jobs.EmployeeRate = job.EmployeeRate;


        await _context.SaveChangesAsync();
                return NoContent();
            }
        }

        private bool BookExists(int id)
        {
            return _context.Job.Any(x => x.Id == id);
        }

    }
}
