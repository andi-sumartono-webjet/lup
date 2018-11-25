namespace Lup.Software.Engineering.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Lup.Software.Engineering.Services.Interface;
    using Microsoft.AspNetCore.Mvc;

    public class RedirectionController : Controller
    {
        private IShortenUrlService urlService;

        public RedirectionController(IShortenUrlService urlService)
        {
            this.urlService = urlService;
        }

        [Route("/r/{url}")]
        public async Task<IActionResult> Index(string url)
        {
            var longUrl = await this.urlService.LongerUrlAsync(url);
            if (!string.IsNullOrEmpty(longUrl))
            {
                return this.Redirect($"http://" + longUrl);
            }
            else
            {
                return this.NotFound();
            }

        }
    }
}