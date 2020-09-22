using System.Collections.Generic;

namespace Presentation.Definition.Dto
{
    public class RepositoryStatsDto
    {
        public ICollection<FileStatsDto> FilesStats { get; set; }
    }
}