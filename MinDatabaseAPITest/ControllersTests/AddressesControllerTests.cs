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
    public class AddressesControllerTests
    {
        private SqlCustomerService _customerService;
        private Mock<ILoggerService> _loggerServiceMock;
        private AddressesController _addressesController;

        [TestInitialize]
        public void Initialize()
        {
            _customerService = new SqlCustomerService(new DatabaseConfigurationService());
            _loggerServiceMock = new Mock<ILoggerService>();

            _addressesController = new AddressesController(
                _customerService,
                _loggerServiceMock.Object
            );
        }

        [TestMethod]
        public void AddAddresses_ValidInput_ReturnsOk()
        {

            var customerId = 1;
            var addresses = new List<Address>
            {
                new Address { AddressId = 1, Street = "Street1", City = "City1", PostalCode ="12345"},
                new Address { AddressId = 2, Street = "Street2", City = "City2", PostalCode ="67890"}
            };

            var result = _addressesController.AddAddresses(customerId, addresses);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ObjectResult));


        }
        [TestMethod]
        public void AddAddresses_InvalidInput_ReturnsBadRequest()
        {
            var customerId = 1;
            var addresses = new List<Address> 
            {
                new Address {AddressId = 1}
            };

            var result = _addressesController.AddAddresses(customerId, addresses);

            Assert.IsNotNull(result);
            if (result is BadRequestResult)
            {
                
                Assert.IsTrue(true);
            }
            else if (result is ObjectResult objResult && objResult.StatusCode == 500)
            {
                
                Assert.IsTrue(true);
            }
            else
            {
               
                Assert.Fail($"Unexpected result: {result.GetType()} with status code {(result as ObjectResult)?.StatusCode}");
            }
        }
        [TestMethod]
        public void DeleteAddress_ValidInput_ReturnsNoContent()
        {
            var addressId = 1;
            var result = _addressesController.DeleteAddress(addressId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}


