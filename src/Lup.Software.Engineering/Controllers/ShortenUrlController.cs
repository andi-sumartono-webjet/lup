namespace Lup.Software.Engineering.Controllers
{
    using System.Threading.Tasks;
    using Lup.Software.Engineering.Services.Interface;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/url")]
    public class ShortenUrlController
    {
        private readonly IShortenUrlService urlService;

        public ShortenUrlController(IShortenUrlService urlService)
        {
            this.urlService = urlService;
        }

        public IUrlHelper UrlHelper { get; }

        [HttpGet("short")]
        public async Task<string> Shorten(string url)
        {
            var shortUrl = await this.urlService.ShortenUrl(url);
            return $"/rd/${shortUrl.RowKey}";
        }      
         
    }
}
