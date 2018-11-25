namespace Lup.Software.Engineering.Controllers
{
    using Lup.Software.Engineering.Services.Interface;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/url")]
    public class ShortenUrlController
    {
        private readonly IShortenUrlService urlService;

        public ShortenUrlController(IShortenUrlService urlService)
        {
            this.urlService = urlService;
        }    

        [HttpGet("short")]
        public async Task<string> Shorten(string url)
        {
            var shortUrl = await this.urlService.ShortenUrl(url);
            return shortUrl.RowKey;
        }        
    }
}
