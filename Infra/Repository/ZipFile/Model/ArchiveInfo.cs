namespace Infra.Repository.ZipFile.Model
{
    public class ArchiveInfo
    {
        public string Name { get; set; }
        public int AmountLines { get; set; }
        public long Bytes { get; set; }

        private string extension;
        public string Extension
        {
            get
            {
                if (extension == null)
                {
                    var nameSplitDot = Name.Split('.');
                    extension = nameSplitDot[nameSplitDot.Length - 1];
                }

                return extension;
            }
        }
    }
}