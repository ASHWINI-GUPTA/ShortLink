using Microsoft.AspNetCore.Mvc;
using ShortLink.Repositories.Interfaces;

namespace ShortLink.Controllers
{
    public class RedirectController : ControllerBase
    {
        private readonly ILinkRepository _linkRepository;

        public RedirectController(ILinkRepository linkRepository)
        {
            _linkRepository = linkRepository;
        }

        [HttpGet("{shortCode}")]
        public IActionResult Get(string shortCode)
        {
            var entity = _linkRepository.Get(shortCode);
            if (entity is null)
                return BadRequest();

            return Redirect(entity.Url);
        }
    }
}
