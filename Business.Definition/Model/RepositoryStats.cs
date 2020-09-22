using System.Collections.Generic;

namespace Business.Definition.Model
{
    public class RepositoryStats
    {
        public ICollection<ArchiveStats> ArchivesStats { get; set; }
    }
}