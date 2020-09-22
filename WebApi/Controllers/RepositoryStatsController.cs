using Microsoft.AspNetCore.Mvc;
using Presentation.Definition.Contract;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RepositoryStatsController : ControllerBase
    {
        private readonly IReposytoryStatsPresentation _reposytoryStatsPresentation;

        public RepositoryStatsController(IReposytoryStatsPresentation reposytoryStatsPresentation)
        {
            _reposytoryStatsPresentation = reposytoryStatsPresentation;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string path, [FromQuery] string branch = "master")
        {
            ObjectResult result = await _reposytoryStatsPresentation.GetAsync(path, branch);

            return result;
        }
    }
}