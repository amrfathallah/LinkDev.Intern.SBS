using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SBS.Application.DTOs.ResourceDto;
using SBS.Application.Interfaces.IServices;
using System;
using System.Threading.Tasks;

namespace SmartBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService _resourceService;
  

        public ResourceController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var resources = await _resourceService.GetAllAsync();
            return Ok(resources);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var resource = await _resourceService.GetByIdAsync(id);
            if (resource == null) return NotFound();
            return Ok(resource);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateResourceDto dto)
        {
            var resource = await _resourceService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = resource.Id }, resource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateResourceDto dto)
        {
            var resource = await _resourceService.UpdateAsync(id, dto);
            if (resource == null) return NotFound();
            return Ok(resource);
        }

        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> Activate(Guid id)
        {
            var result = await _resourceService.SetActiveStatusAsync(id, true);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var result = await _resourceService.SetActiveStatusAsync(id, false);
            if (!result) return NotFound();
            return NoContent();
        }
    }
} 