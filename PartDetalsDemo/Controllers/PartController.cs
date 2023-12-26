using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PartDetailsDemo.CQRS.Commands;
using PartDetailsDemo.CQRS.Queries;
using Microsoft.AspNetCore.Authorization;
using PartDetailsDemo.Models;
using System.Security.Claims;
using PartDetailsDemo.Repositories;
using Microsoft.Extensions.Logging;

namespace PartDetailsDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartController : ControllerBase
    {
        private IMediator mediator;
        //private readonly ILogger<PartRepository> seriLogger;
        public PartController(IMediator mediator)
        {
            this.mediator = mediator;
            //this.seriLogger = seriLogger;
        }

        [HttpPost]
        [Authorize(Roles = "Contribute,FullAccess")]
        public async Task<IActionResult> Create(CreatePartCommand command)
        {
            try
            {
                var partCreate = await mediator.Send(command);
                if (partCreate == 1)
                {
                    return Ok("Data is created successfully.");
                }
                else
                {
                    return BadRequest("Data is not created.");
                }
            }
            catch
            {
                return StatusCode(500, "Time Stamp: " + DateTime.Now.ToString("G") + " An error occured, please contact admin.");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Read,Contribute,FullAccess")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var part = await mediator.Send(new GetAllPartQuery());
                var parts = part as List<Part>;

                if(parts.Count == 0)
                {
                    return NoContent();

                }

                return Ok(part);
            }
            catch
            {
                return StatusCode(500, "Time Stamp: " + DateTime.Now.ToString("G") + " An error occured, please contact admin.");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Read,Contribute,FullAccess")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var partDetails = await mediator.Send(new GetPartByIdQuery { Id = id });
                if (partDetails != null)
                {
                    return Ok(partDetails);
                }
                return NotFound("Part not found.");
            }
            catch
            {
                return StatusCode(500, "Time Stamp: " + DateTime.Now.ToString("G") + "An error occured, please contact admin.");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Contribute,FullAccess")]
        public async Task<IActionResult> Update(int id, UpdatePartCommand command)
        {
            try
            {
                command.PartId = id;
                var partUpdate = await mediator.Send(command);
                if (partUpdate == 1)
                {
                    return Ok("Data is updated successfully.");
                }
                else
                {
                    return Ok("Data not found.");
                }
            }
            catch
            {
                return StatusCode(500, "Time Stamp: " + DateTime.Now.ToString("G") + " An error occured, please contact admin.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "FullAccess")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var partDelete = await mediator.Send(new DeletePartByIdCommand { PartId = id });
                if (partDelete == 1)
                {
                    return Ok("Data is deleted successfully.");
                }
                else
                {
                    return Ok("Data not found.");
                }
            }
            catch
            {
                return StatusCode(500, "Time Stamp: " + DateTime.Now.ToString("G") + "- An error occured, please contact admin.");
            }
        }
    }
}
