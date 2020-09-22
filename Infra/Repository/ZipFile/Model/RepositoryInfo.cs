using Business.Definition.Model;
using System.Net.Http.Headers;

namespace Infra.Repository.ZipFile.Model
{
    public class RepositoryInfo
    {
        public EntityTagHeaderValue ETag { get; set; }
        public RepositoryStats RepositoryStats { get; set; }
    }
}