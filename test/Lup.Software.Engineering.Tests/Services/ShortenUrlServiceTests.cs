namespace Lup.Software.Engineering.Tests.Services
{
    using Lup.Software.Engineering.Models;
    using Lup.Software.Engineering.Repositories.Interface;
    using Lup.Software.Engineering.Services;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class ShortenUrlServiceTests
    {
        private readonly Mock<ILogger<ShortenUrlService>> _logger;
        private readonly Mock<ITableRepository<ShortUrl>> _repository;
        private readonly ShortenUrlService _service;

        public ShortenUrlServiceTests()
        {
            this._logger = new Mock<ILogger<ShortenUrlService>>();
            this._repository = new Mock<ITableRepository<ShortUrl>>();
            this._service = new ShortenUrlService(this._repository.Object, this._logger.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task ShortenUrl_ShouldReturn_AShortStringAsync()
        {
            // arrange
            var longUrl = "www.google.com/adfads";
            this._repository.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns<string>(null);

            // act
            ShortUrl shortUrl = await this._service.ShortenUrl(longUrl);

            // assert
            Assert.NotNull(shortUrl);
            Assert.Equal(5, shortUrl.RowKey.Length);

        }
    }
}
