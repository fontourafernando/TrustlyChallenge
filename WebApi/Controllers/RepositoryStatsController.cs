using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Definition.Commom;
using Presentation.Definition.Contract;
using Presentation.Definition.Dto;
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

        /// <summary>
        /// Get the Stats of the GitHub Repository by path.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /RepositoryStats?path=/fontourafernando/TrustlyChallenge
        ///
        /// </remarks>
        /// <param name="path">The repository path. Initialize with /.</param>
        /// <param name="branch">The branch repository. Default is 'master'.</param>
        /// <returns>The amount bityes and lines group by extension flile.</returns>
        /// <response code="200">Returns the stats</response>
        /// <response code="400">If occur any error</response> 
        [Produces("application/json")]
        [HttpGet]
        [ProducesResponseType(typeof(RepositoryStatsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromQuery]string path, [FromQuery] string branch = "master")
        {
            ObjectResult result = await _reposytoryStatsPresentation.GetAsync(path, branch);

            return result;
        }
    }
}