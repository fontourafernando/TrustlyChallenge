using Business.Definition.Model;
using System.Threading.Tasks;

namespace Business.Definition.Infra.Repository
{
    public interface IRepositoryStatsRepository
    {
        Task<RepositoryStats> GetByRepositoryPathAsync(string path, string branch = "master");
    }
}