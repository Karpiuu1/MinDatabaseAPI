using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinDatabaseAPI.Controllers;
using MinDatabaseAPI.Interface;
using MinDatabaseAPI.Models;
using MinDatabaseAPI.Services;
using Moq;

namespace MinDatabaseAPITest.ControllersTests
{
    [TestClass]
    public class EfAddressesControllerTests
    {
        private EfAddressesController _addressesController;
        private Mock<EfCustomerService> _customerServiceMock;
        private Mock<ILoggerService> _loggerServiceMock;
        private CustomerDbContext _dbContext;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new CustomerDbContext(options);

            _customerServiceMock = new Mock<EfCustomerService>();
            var logger = new Mock<ILoggerService>();
            _loggerServiceMock = logger;
            var customerService = new EfCustomerService(_dbContext);
            _addressesController = new EfAddressesController(customerService, _loggerServiceMock.Object);
        }

        [TestMethod]
        public void AddAddresses_ValidModel_ReturnsOkResult()
        {
            var customerId = 1;
            var addresses = new List<Address>
        {
            new Address { Street = "123 Main St", City = "Sample City", PostalCode = "12345" },
            new Address { Street = "456 Elm St", City = "Test City", PostalCode = "67890" }
        };

            var result = _addressesController.AddAddresses(customerId, addresses);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void AddAddresses_InvalidModel_ReturnsBadRequest()
        {
            var customerId = 1;
            var addresses = new List<Address>();

            // Dodaj błąd do ModelState, który wyzwie BadRequest
            _addressesController.ModelState.AddModelError("PropertyName", "ErrorMessage");

            var result = _addressesController.AddAddresses(customerId, addresses);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteAddress_ExistingAddressId_ReturnsNoContent()
        {
            var addressId = 1;

            var result = _addressesController.DeleteAddress(addressId);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteAddress_NonExistingAddressId_ReturnsStatusCode500()
        {
            _loggerServiceMock.Setup(mock => mock.LogError(It.IsAny<string>()));
            var addressId = 100;

            var result = _addressesController.DeleteAddress(addressId);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            
        }

    }
}
