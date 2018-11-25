namespace Lup.Software.Engineering.Services
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using Lup.Software.Engineering.Models;
    using Lup.Software.Engineering.Repositories.Interface;
    using Lup.Software.Engineering.Services.Interface;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage.Table;

    public class ShortenUrlService : IShortenUrlService
    {
        private readonly ITableRepository<ShortUrl> urlRepository;
        private readonly ILogger<ShortenUrlService> logger;

        public ShortenUrlService(ITableRepository<ShortUrl> urlRepository, ILogger<ShortenUrlService> logger)
        {
            this.urlRepository = urlRepository;
            this.logger = logger;
        }

        public async Task<string> LongerUrlAsync(string shortUrl)
        {
            if (string.IsNullOrEmpty(shortUrl)
                || shortUrl.StartsWith("http://")
                || shortUrl.StartsWith("https://")
                || shortUrl.StartsWith("ftp://"))
            {
                throw new System.UriFormatException("input not in the correct format");
            }

            var results = await this.urlRepository.QueryAsync(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, shortUrl));
            if (results != null && results.Count() > 0)
            {
                return results.First().OriginalUrl;
            }
            else
            {
                this.logger.LogError($"cannot find a longer version of ${shortUrl}");
                return string.Empty;
            }
        }


        private string GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create(); 
            byte[] encoded = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            return Encoding.Default.GetString(encoded);
        }

        public async Task<ShortUrl> ShortenUrl(string longUrl)
        {
            if (string.IsNullOrEmpty(longUrl) 
                || longUrl.StartsWith("http://") 
                || longUrl.StartsWith("https://")
                || longUrl.StartsWith("ftp://"))
            {
                throw new System.UriFormatException("input not in the correct format");
            }

            var partitionKey = longUrl.Split("/").First();
            var rowKey = this.GetHash(longUrl).Substring(0, 5);

            var existingData = await this.urlRepository.Get(partitionKey, rowKey);
            if (existingData == null)
            {
                var newUrl = new ShortUrl
                {
                    PartitionKey = partitionKey,
                    RowKey = rowKey,
                    Count = 1,
                    OriginalUrl = longUrl
                };

                return await this.urlRepository.AddAsync(newUrl);
            } 
            else
            {
                existingData.Count += 1;
                return await this.urlRepository.UpdateAsync(existingData);
            }

        }
    }
}
