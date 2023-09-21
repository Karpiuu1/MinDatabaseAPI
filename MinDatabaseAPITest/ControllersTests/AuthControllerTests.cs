using Microsoft.Extensions.Configuration;
using MinDatabaseAPI.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MinDatabaseAPI.Services;
using MinDatabaseAPI.Interface;
using MinDatabaseAPI.Models;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace MinDatabaseAPITest.ControllersTests
   
{
    [TestClass]
    public class AuthControllerTests
    {
        private AuthController _authController;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;

        public AuthControllerTests() 
        {
            _configurationMock = new Mock<IConfiguration>();
            _loggerServiceMock = new Mock<ILoggerService>();

            var configService = new DatabaseConfigurationService();
            var administrationService = new SqlAdministrationService(configService); 

            _authController = new AuthController(
                _configurationMock.Object,
                administrationService,
                _loggerServiceMock.Object);
        }

        [TestMethod]
        public async Task Register_ValidUser_ReturnsOkResult()
        {
            var userDto = new UserDto
            {
                Username = "testuser",
                Password = "password",
                Role = "user"
            };


            var result = await _authController.Register(userDto);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ActionResult<Administration>));
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }
        [TestMethod]
        public async Task Register_InvalidUser_ReturnsInternalSeverError()
        {
            var userDto = new UserDto
            {
                Username = "testuser",
                Password = "",
                Role = "user"
            };


            var result = await _authController.Register(userDto);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result.Result;
            var errorMessage = okResult.Value as string;
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage) || string.IsNullOrWhiteSpace(errorMessage));
        }
    }
}
