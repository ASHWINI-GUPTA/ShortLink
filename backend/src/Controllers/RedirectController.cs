using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShortLink.Services.Interface;

namespace ShortLink.Controllers
{
    public class RedirectController : ControllerBase
    {
        private readonly ILinkService _linkService;

        public RedirectController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> Get(string shortCode)
        {
            var entity = await _linkService.Get(shortCode);
            if (entity is null)
                return BadRequest();

            return Redirect(entity.Url);
        }
    }
}
