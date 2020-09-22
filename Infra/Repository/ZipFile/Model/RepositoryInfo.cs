using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Infra.Repository.ZipFile.Model
{
    public class RepositoryInfo
    {
        public EntityTagHeaderValue ETag { get; set; }
        public ICollection<ArchiveInfo> Archives { get; set; }
    }
}