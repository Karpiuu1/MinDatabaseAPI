﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MinDatabaseAPI.Models;

namespace MinDatabaseAPI.Services
{
    public class SqlCustomerService
    {
        private readonly string _connectionString;

        public SqlCustomerService(DatabaseConfigurationService configService)
        {
            _connectionString = configService.GetConnectionString();
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.Query<Customer>("SELECT * FROM Customers");
        }

        public Customer GetCustomerById(int id)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.QuerySingleOrDefault<Customer>("SELECT * FROM Customers WHERE Id = @Id", new { Id = id });
        }


        public IEnumerable<Address> GetAddressesByCustomerId(int customerId)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.Query<Address>("SELECT * FROM Addresses WHERE CustomerId = @CustomerId", new {CustomerId = customerId});
        }

        public int InsertCustomer(Customer customer)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            const string query = @"
                INSERT INTO Customers (FirstName, LastName, Username, Password, Role)
                VALUES (@FirstName, @LastName, @Username, @Password, @Role);
                SELECT CAST (SCOPE_IDENTITY() AS INT)";
            return db.Query<int>(query, customer).Single();
        }

        public void InsertAddresses(int customerId, IEnumerable<Address> addresses)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            const string query = @"
                INSERT INTO Addresses (CustomerId, Street, City, PostalCode)
                VALUES (@CustomerId, @Street, @City, @PostalCode)";
            foreach (var  address in addresses)
            {
                address.CustomerId = customerId;
                db.Execute(query, address);
            }
        }

        public void DeleteAddress(int addressId)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            const string query = @"DELETE FROM Addresses WHERE AddressId = @AddressId";
            db.Execute(query, new {AddressId = addressId});
        }

        public void DeleteCustomer(int customerId)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            const string query = @"DELETE FROM Customers WHERE Id = @CustomerId";
            db.Execute(query, new {CustomerId = customerId});
        }
    }
}
