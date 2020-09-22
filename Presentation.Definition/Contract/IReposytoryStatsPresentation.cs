using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Presentation.Definition.Contract
{
    public interface IReposytoryStatsPresentation
    {
        Task<ObjectResult> GetAsync(string path, string brach = "master");
    }
}