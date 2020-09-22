using Business.Definition.Contract;
using Business.Definition.Model;
using Microsoft.AspNetCore.Mvc;
using Presentation.Definition.Commom;
using Presentation.Definition.Contract;
using Presentation.Definition.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation
{
    public class RepositoryStatsPresentation : IReposytoryStatsPresentation
    {
        private readonly IRepositoryStatsCore _repositoryStatsCore;

        public RepositoryStatsPresentation(IRepositoryStatsCore repositoryStatsCore)
        {
            _repositoryStatsCore = repositoryStatsCore;
        }

        public async Task<ObjectResult> GetAsync(string path, string brach = "master")
        {
            Result result = new Result();
            try
            {
                RepositoryStats repositoryStats = await _repositoryStatsCore.GetAsync(path, brach);

                RepositoryStatsDto repositoryStatsDto = MapTo(repositoryStats);

                result.Value = repositoryStatsDto;

                result.Success = true;
            }
            catch (Exception e)
            {
                result.Error = new ErrorDto
                {
                    ErrorType = e.GetType().Name,
                    Message = e.Message
                };
            }

            return result.Return();
        }

        private RepositoryStatsDto MapTo(RepositoryStats repositoryStats)
        {
            ICollection<FileStatsDto> fileStatsDtos = repositoryStats.ArchivesStats
                .Select(fs => new FileStatsDto
                {
                    Extension = fs.Extension,
                    AmountLines = fs.AmountLines,
                    AmountBytes = fs.AmountBytes,
                })
                .ToList();

            RepositoryStatsDto repositoryStatsDto = new RepositoryStatsDto
            {
                FilesStats = fileStatsDtos
            };

            return repositoryStatsDto;
        }
    }
}