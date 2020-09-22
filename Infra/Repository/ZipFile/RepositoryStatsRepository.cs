using Business.Definition.Infra.Repository;
using Business.Definition.Model;
using Infra.Common;
using Infra.Repository.ZipFile.Model;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Infra.Repository.ZipFile.GitHub
{
    public class RepositoryStatsRepository : IRepositoryStatsRepository, IDisposable
    {
        private const string _gitHubRepositoryZipFileUriPattern = "https://github.com{0}/archive/{1}.zip";

        private readonly IMemoryCache _memoryCache;
        private readonly HttpClient _httpClient;

        public RepositoryStatsRepository(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _memoryCache = memoryCache;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<RepositoryStats> GetByRepositoryPathAsync(string path, string branch = "master")
        {
            if (path == null) throw new ArgumentNullException(nameof(path), $"The {nameof(path)} parameter can not be null.");
            if (branch == null) throw new ArgumentNullException(nameof(branch), $"The {nameof(branch)} parameter can not be null.");

            var cacheKey = $"{MethodBase.GetCurrentMethod().ReflectedType.GUID}_{path}";

            RepositoryInfo repositoryInfoGH = _memoryCache.Get<RepositoryInfo>(cacheKey);

            if (repositoryInfoGH != null)
                _httpClient.DefaultRequestHeaders.IfNoneMatch.Add(repositoryInfoGH.ETag);

            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(string.Format(_gitHubRepositoryZipFileUriPattern, path, branch));

            if (httpResponseMessage.StatusCode.Equals(HttpStatusCode.OK))
            {
                var filesInfoGH = await GetAsync(httpResponseMessage);

                RepositoryStats repositoryStats = MapTo(filesInfoGH);

                repositoryInfoGH = new RepositoryInfo
                {
                    ETag = httpResponseMessage.Headers.ETag,
                    RepositoryStats = repositoryStats
                };

                _memoryCache.Set(cacheKey, repositoryInfoGH);
            }
            else if (!httpResponseMessage.StatusCode.Equals(HttpStatusCode.NotModified))
                throw new RepositoryException("GitHub Repository not found.");

            return repositoryInfoGH.RepositoryStats;
        }

        private async Task<ICollection<ArchiveInfo>> GetAsync(HttpResponseMessage httpResponseMessage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await httpResponseMessage.Content.CopyToAsync(memoryStream);

                ICollection<ArchiveInfo> archivesInfo = await GetAsync(memoryStream);

                return archivesInfo;
            }
        }

        private async Task<ICollection<ArchiveInfo>> GetAsync(MemoryStream memoryStream)
        {
            using (ZipArchive zipArchive = new ZipArchive(memoryStream))
            {
                ICollection<ArchiveInfo> arquivesInfo = await GetAsync(zipArchive);

                return arquivesInfo;
            }
        }


        private async Task<ICollection<ArchiveInfo>> GetAsync(ZipArchive zipArchive)
        {
            ICollection<ArchiveInfo> arquivesInfo = new List<ArchiveInfo>();

            if (zipArchive != null)
            {
                foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries)
                {
                    if (!zipArchiveEntry.Name.Equals("") && !zipArchiveEntry.Name.EndsWith("/"))
                    {
                        int amountLines;

                        using (Stream streamArchive = zipArchiveEntry.Open())
                        {
                            amountLines = await GetAmountLinesAsync(streamArchive);
                        }

                        ArchiveInfo arquiveInfo = new ArchiveInfo
                        {
                            Name = zipArchiveEntry.Name,
                            Bytes = zipArchiveEntry.Length,
                            AmountLines = amountLines
                        };

                        arquivesInfo.Add(arquiveInfo);
                    }
                }
            }

            return arquivesInfo;
        }

        private async Task<int> GetAmountLinesAsync(Stream streamArchive)
        {
            using (StreamReader streamReaderArchive = new StreamReader(streamArchive))
            {
                int amountLines = await GetAmountLinesAsync(streamReaderArchive);

                return amountLines;
            }
        }

        private async Task<int> GetAmountLinesAsync(StreamReader streamReaderArchive)
        {
            string archiveContent = await streamReaderArchive.ReadToEndAsync();
            var lines = archiveContent.Split("\n");

            return lines.Length;
        }

        private RepositoryStats MapTo(ICollection<ArchiveInfo> archivesInfo)
        {
            RepositoryStats repositoryStats = null;

            if (archivesInfo != null)
            {
                ICollection<ArchiveStats> filesStats = archivesInfo
                    .GroupBy(f => f.Extension)
                    .Select(g => new ArchiveStats
                    {
                        Extension = g.Key,
                        AmountBytes = g.Sum(s => s.Bytes),
                        AmountLines = g.Sum(s => s.AmountLines)
                    })
                    .OrderByDescending(f => f.AmountBytes)
                    .ThenByDescending(f => f.AmountLines)
                    .ThenByDescending(f => f.Extension)
                    .ToList(
                    );

                repositoryStats = new RepositoryStats
                {
                    ArchivesStats = filesStats
                };
            }

            return repositoryStats;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}