using System;

namespace Presentation.Definition.Dto
{
    public class FileStatsDto
    {
        public string Extension { get; set; }
        public long AmountLines { get; set; }
        public long AmountBytes { get; set; }

        private string humanAmountBytes;
        public string HumanAmountBytes
        {
            get
            {
                if (humanAmountBytes == null)
                {
                    humanAmountBytes = ToHumanBytes();
                }

                return humanAmountBytes;
            }
        }

        private string ToHumanBytes()
        {
            string value;
            string unit;

            if (AmountBytes < 1024)
            {
                value = AmountBytes.ToString("0.##");
                unit = "Bytes";
            }
            else if (AmountBytes >= 1024 && AmountBytes < Math.Pow(1024, 2))
            {
                value = ((double)AmountBytes / 1024).ToString("0.##");
                unit = "KB";
            }
            else if (AmountBytes >= Math.Pow(1024, 2) && AmountBytes < Math.Pow(1024, 3))
            {
                value = ((double)AmountBytes / Math.Pow(1024, 2)).ToString("0.##");
                unit = "MB";
            }
            else
            {
                value = ((double)AmountBytes / Math.Pow(1024, 3)).ToString("0.##");
                unit = "GB";
            }

            return $"{value} {unit}";
        }
    }
}
