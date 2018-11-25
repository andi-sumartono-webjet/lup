namespace Lup.Software.Engineering.Tests.Controllers
{
    using Lup.Software.Engineering.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    public class HomeControllerTests
    {
        private HomeController controller;

        public HomeControllerTests()
        {
            this.controller = new HomeController();
        }

        [Fact]
        public void IndexShouldReturnViewResult()
        {
            var result = this.controller.Index();

            Assert.IsType<ViewResult>(result);
        }
    }
}