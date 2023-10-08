using MinDatabaseAPI.Models;
using MinDatabaseAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinDatabaseAPITest.ServicesTests
{
    [TestClass]
    public class SqlCustomerServiceTests
    {
        private SqlCustomerService _customerService;
        private string _connectionString;

        [TestInitialize]
        public void Initialize()
        {
            _connectionString = "Server=(localdb)\\mssqllocaldb;Database=MinDatabase;Trusted_Connection=True;";
            var connectionSettings = new DatabaseConfigurationService();
            _customerService = new SqlCustomerService(connectionSettings);
        }
        [TestMethod]
        public void GetAllCustomers_ReturnsCustomers()
        {
            var customers = _customerService.GetAllCustomers();
            Assert.IsNotNull(customers);
            Assert.IsTrue(customers.Count() > 0);
        }
        [TestMethod]
        public void GetCustomerById_ExistingId_ReturnsCustomer()
        {
            var newCustomer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe",
                Password = "password",
                Role = "User"
            };

            var newCustomerId = _customerService.InsertCustomer(newCustomer);

            var customer = _customerService.GetCustomerById(newCustomerId);

            Assert.IsNotNull(customer);
            Assert.AreEqual(newCustomerId, customer.Id);
            Assert.AreEqual(newCustomer.FirstName, customer.FirstName);
        }
        [TestMethod]
        public void GetCustomerById_NonExistingId_ReturnsNull()
        {
            var customer = _customerService.GetCustomerById(-1);

            Assert.IsNull(customer);
        }
        [TestMethod]
        public void GetAddressesByCustomerId_ExistingCustomerId_ReturnsAddresses()
        {
            var newCustomer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe",
                Password = "password",
                Role = "User"
            };

            var newCustomerId = _customerService.InsertCustomer(newCustomer);

            var addresses = new List<Address>
        {
            new Address
            {
                CustomerId = newCustomerId,
                Street = "123 Main St",
                City = "City1",
                PostalCode = "12345"
            },
            new Address
            {
                CustomerId = newCustomerId,
                Street = "456 Elm St",
                City = "City2",
                PostalCode = "67890"
            }
        };

            _customerService.InsertAddresses(newCustomerId, addresses);

            var customerAddresses = _customerService.GetAddressesByCustomerId(newCustomerId);

            Assert.IsNotNull(customerAddresses);
            Assert.IsTrue(customerAddresses.Count() > 0);
        }
        [TestMethod]
        public void GetAddressesByCustomerId_NonExistingCustomerId_ReturnsEmptyList()
        {
            var customerAddresses = _customerService.GetAddressesByCustomerId(-1);

            Assert.IsNotNull(customerAddresses);
            Assert.AreEqual(0, customerAddresses.Count());
        }
        [TestMethod]
        public void InsertCustomer_ValidData_ReturnsNewId()
        {
            var newCustomer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe",
                Password = "password",
                Role = "User"
            };

            var newCustomerId = _customerService.InsertCustomer(newCustomer);

            Assert.IsTrue(newCustomerId > 0);
        }
        [TestMethod]
        public void InsertAddresses_ValidData_ReturnsNoException()
        {
            var newCustomer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe",
                Password = "password",
                Role = "User"
            };

            var newCustomerId = _customerService.InsertCustomer(newCustomer);

            var addresses = new List<Address>
        {
            new Address
            {
                CustomerId = newCustomerId,
                Street = "123 Main St",
                City = "City1",
                PostalCode = "12345"
            },
            new Address
            {
                CustomerId = newCustomerId,
                Street = "456 Elm St",
                City = "City2",
                PostalCode = "67890"
            }
        };

            try
            {
                _customerService.InsertAddresses(newCustomerId, addresses);
            }
            catch (Exception)
            {
                Assert.Fail("InsertAddresses should not throw an exception.");
            }
        }
        [TestMethod]
        public void DeleteAddress_ValidAddressId_ReturnsNoException()
        {
            var newCustomer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe",
                Password = "password",
                Role = "User"
            };

            var newCustomerId = _customerService.InsertCustomer(newCustomer);

            var address = new Address
            {
                CustomerId = newCustomerId,
                Street = "123 Main St",
                City = "City1",
                PostalCode = "12345"
            };

            _customerService.InsertAddresses(newCustomerId, new List<Address> { address });

            try
            {
                _customerService.DeleteAddress(address.AddressId);
            }
            catch (Exception)
            {
                Assert.Fail("DeleteAddress should not throw an exception.");
            }
        }
        [TestMethod]
        public void DeleteAddress_NonExistingAddressId_ReturnsNoException()
        {
            try
            {
                _customerService.DeleteAddress(-1);
            }
            catch (Exception)
            {
                Assert.Fail("DeleteAddress should not throw an exception for non-existing address.");
            }
        }

        [TestMethod]
        public void DeleteCustomer_ValidCustomerId_ReturnsNoException()
        {
            var newCustomer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe",
                Password = "password",
                Role = "User"
            };

            var newCustomerId = _customerService.InsertCustomer(newCustomer);

            try
            {
                _customerService.DeleteCustomer(newCustomerId);
            }
            catch (Exception)
            {
                Assert.Fail("DeleteCustomer should not throw an exception.");
            }
        }

        [TestMethod]
        public void DeleteCustomer_NonExistingCustomerId_ReturnsNoException()
        {
            try
            {
                _customerService.DeleteCustomer(-1);
            }
            catch (Exception)
            {
                Assert.Fail("DeleteCustomer should not throw an exception for non-existing customer.");
            }
        }


    }
}

