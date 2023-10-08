using Microsoft.AspNetCore.Mvc;
using MinDatabaseAPI.Controllers;
using MinDatabaseAPI.Interface;
using MinDatabaseAPI.Models;
using MinDatabaseAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinDatabaseAPITest.ControllersTests
{
    [TestClass]
    public class CustomersControllerTests
    {
        private SqlCustomerService _customerService;
        private Mock<ILoggerService> _loggerServiceMock;
        private CustomersController _customerController;

        [TestInitialize]
        public void Initialize()
        {
            _customerService = new SqlCustomerService(new DatabaseConfigurationService());
            _loggerServiceMock = new Mock<ILoggerService>();
            _customerController = new CustomersController(
                _customerService,
                _loggerServiceMock.Object);
        }

        [TestMethod]
        public void GetAllCustomers_ReturnsOk()
        {
            var result = _customerController.GetAllCustomers();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetCustomerById_ValidId_ReturnsOk()
        {
            var customerId = 2;
            var result = _customerController.GetCustomerById(customerId);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetCustomerById_InvalidId_ReturnsNotFound()
        {
            var customerId = -1;
            var result = _customerController.GetCustomerById(customerId);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void AddCustomer_ValidInpit_ReturnsCreatedAtAction()
        {
            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe",
                Password = "password",
                Role = "User"
            };

            var result = _customerController.AddCustomer(customer);

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void AddCustomer_InvalidInput_ReturnsBadRequest()
        {
            var customer = new Customer();
            _customerController.ModelState.AddModelError("PropertyName", "ErrorMessage");

       
            var result = _customerController.AddCustomer(customer);

      
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteCustomer_ValidId_ReturnsNoContent()
        {
            var customerId = 11;

            var result = _customerController.DeleteCustomer(customerId);

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
        }

        [TestMethod]
        public void DeleteCustomer_InvalidId_ReturnsNotFound()
        {
            var customerId = -1;

            var result = _customerController.DeleteCustomer(customerId);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
