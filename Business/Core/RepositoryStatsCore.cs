using Business.Definition.Contract;
using Business.Definition.Infra.Repository;
using Business.Definition.Model;
using System;
using System.Threading.Tasks;

namespace Business.Core
{
    public class RepositoryStatsCore : IRepositoryStatsCore
    {
        private readonly IRepositoryStatsRepository _repositoryStatsRepository;

        public RepositoryStatsCore(IRepositoryStatsRepository repositoryStatsRepository)
        {
            _repositoryStatsRepository = repositoryStatsRepository;
        }

        public async Task<RepositoryStats> GetAsync(string path, string brach = "master")
        {
            if (path == null) throw new ArgumentNullException(nameof(path), $"The {nameof(path)} parameter can not be null.");
            if (brach == null) throw new ArgumentNullException(nameof(brach), $"The {nameof(brach)} parameter can not be null.");

            RepositoryStats repositoryStats = await _repositoryStatsRepository.GetByRepositoryPathAsync(path, brach);

            return repositoryStats;
        }
    }
}