using Linkdeed.Data;
using Linkdeed.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        [HttpGet]
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

        // GET: JobsController/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJobs_ById(int id)
        {
            return _context.Job.ToList().Find(x => x.Id == id);
        }

        // GET: JobsController/Create  
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


        [HttpDelete("{id}")]
        public async Task<ActionResult<Job>> Delete_Job(int id)
        {
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

        [HttpPut("{id}")]
        public async Task<ActionResult> Update_Books(int id, Job job)
        {
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
