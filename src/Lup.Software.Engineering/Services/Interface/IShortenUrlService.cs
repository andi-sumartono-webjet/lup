namespace Lup.Software.Engineering.Services.Interface
{ 
    using System.Threading.Tasks;
    using Lup.Software.Engineering.Models;

    public interface IShortenUrlService
    {
        Task<ShortUrl> ShortenUrl(string longUrl);

        Task<string> LongerUrlAsync(string shortUrl);
    }
}
