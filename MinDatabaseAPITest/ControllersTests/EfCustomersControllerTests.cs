using Microsoft.EntityFrameworkCore;
using MinDatabaseAPI.Controllers;
using MinDatabaseAPI.Interface;
using MinDatabaseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MinDatabaseAPI.Services;

namespace MinDatabaseAPITest.ControllersTests
{
    [TestClass]
    public class EfCustomersControllerTests
    {
        private EfCustomersController _customersController;
        private CustomerDbContext dbContext;
        private Mock<ILoggerService> _loggerServiceMock;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            dbContext = new CustomerDbContext(options);


            var customers = new List<Customer>
        {
            new Customer { Username = "Jdoe", FirstName = "John", LastName = "Doe", Password ="pass1", Role = "User" },
            new Customer { Username = "Jsmith", FirstName = "Jane", LastName = "Smith", Password ="pass2", Role ="User" }
        };
            dbContext.Customers.AddRange(customers);
            dbContext.SaveChanges();


            _loggerServiceMock = new Mock<ILoggerService>();


            var customerService = new EfCustomerService(dbContext);
            _customersController = new EfCustomersController(customerService, _loggerServiceMock.Object);
        }

        [TestMethod]
        public void GetAllCustomers_ReturnsOkResult()
        {
            var result = _customersController.GetAllCustomers();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetCustomerById_ExistingId_ReturnsOkResult()
        {
            var result = _customersController.GetCustomerById(1);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetCustomerById_NonExistingId_ReturnsNotFound()
        {
            var result = _customersController.GetCustomerById(100);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetAddressesByCustomerId_ReturnsOkResult()
        {
            var result = _customersController.GetAddressesByCustomerId(1);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void AddCustomer_ValidModel_ReturnsCreatedAtAction()
        {
            var customer = new Customer { FirstName = "Test", LastName = "Test", };
            
            var result = _customersController.AddCustomer(customer);

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
        }

        [TestMethod]
        public void AddCustomer_InvalidModel_ReturnsBadRequest()
        {
            var customer = new Customer();
            _customersController.ModelState.AddModelError("PropertyName", "ErrorMessage");

            var result = _customersController.AddCustomer(customer);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void AddCustomer_ExceptionThrown_ReturnsStatusCode500()
        {
            _loggerServiceMock.Setup(mock => mock.LogError(It.IsAny<string>()));

            var customer = new Customer { FirstName = "Test", LastName = "Test", };

            var result = _customersController.AddCustomer(customer);

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual((result as ObjectResult).StatusCode, 500);
        }
    }
}
