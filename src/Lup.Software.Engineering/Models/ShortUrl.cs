using Microsoft.WindowsAzure.Storage.Table;

namespace Lup.Software.Engineering.Models
{
    public class ShortUrl : TableEntity
    {
        public string OriginalUrl { get; set; }

        public int CountShort { get; set; }

        public int CountLong { get; set; }
    }
}
