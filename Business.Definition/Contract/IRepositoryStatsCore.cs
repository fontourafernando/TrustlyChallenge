using Business.Definition.Model;
using System.Threading.Tasks;

namespace Business.Definition.Contract
{
    public interface IRepositoryStatsCore
    {
        Task<RepositoryStats> GetAsync(string path, string brach = "master");
    }
}