﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShortLink.Models.Request;
using ShortLink.Services.Interface;

namespace ShortLink.Controllers
{
    [ApiController]
    [Route("links")]
    public class LinkController : ControllerBase
    {
        private readonly ILinkService _linkService;

        public LinkController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_linkService.GetAll());
        }

        [HttpGet("{shortCode}")]
        public IActionResult Get(string shortCode)
        {
            return Ok(_linkService.Get(shortCode));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LinkRequest linkRequest)
        {
            var shortCode = await _linkService.Create(linkRequest);
            return Ok(new {ShortCode = shortCode});
        }

        [HttpPut("{shortCode}")]
        public async Task<IActionResult> Update(string shortCode, [FromBody] LinkRequest linkRequest)
        {
            linkRequest.ShortCode = shortCode;
            await _linkService.Update(linkRequest);
            return Ok(new {ShortCode = shortCode});
        }

        [HttpDelete("{shortCode}")]
        public async Task<IActionResult> Delete(string shortCode)
        {
            await _linkService.Delete(shortCode);
            return Ok(new {ShortCode = shortCode});
        }
    }
}