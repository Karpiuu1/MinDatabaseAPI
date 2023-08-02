using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Net;
using MinDatabaseAPI.Entities;

namespace MinDatabaseAPI.Services;
public class CustomerService
{
    private readonly string connectionString;

    public CustomerService(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public List<Customer> GetAllCustomers()
    {
        List<Customer> customers = new List<Customer>();
        string query = "SELECT * FROM Customers;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer customer = new Customer
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                            Role = reader["Role"].ToString()
                        };
                        customers.Add(customer);
                    }
                }
            }
        }

        return customers;
    }

    public Customer GetCustomerById(int id)
    {
        Customer customer = null;
        string query = "SELECT * FROM Customers WHERE ID = @id;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        customer = new Customer
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                            Role = reader["Role"].ToString()
                        };
                    }
                }
            }
        }
        return customer;
    }

    public List<Address> GetAddressesByCustomerId(int customerId)
    {
        List<Address> addresses = new List<Address>();
        string query = "SELECT * FROM ShippingAddresses WHERE CustomerID = @customerId;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@customerId", customerId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Address address = new Address
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            CustomerID = Convert.ToInt32(reader["CustomerID"]),
                            AddressText = reader["Address"].ToString(),
                            City = reader["City"].ToString(),
                            PostalCode = reader["PostalCode"].ToString(),
                            Country = reader["Country"].ToString()
                        };
                        addresses.Add(address);
                    }
                }
            }
        }

        return addresses;
    }

    public void InsertNewCustomer(Customer customer)
    {
        string query = "INSERT INTO Customers (FirstName, LastName, Username, Password, Role) " +
                       "VALUES (@firstName, @lastName, @username, @password, @role);";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@firstName", customer.FirstName);
                command.Parameters.AddWithValue("@lastName", customer.LastName);
                command.Parameters.AddWithValue("@username", customer.Username);
                command.Parameters.AddWithValue("@password", customer.Password);
                command.Parameters.AddWithValue("@role", customer.Role);

                command.ExecuteNonQuery();
            }
        }
    }

    public void InsertNewAddressesForCustomer(int customerId, List<Address> addresses)
    {
        string query = "INSERT INTO ShippingAddresses (CustomerID, Address, City, PostalCode, Country) " +
                       "VALUES (@customerId, @address, @city, @postalCode, @country);";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            foreach (Address address in addresses)
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@customerId", customerId);
                    command.Parameters.AddWithValue("@address", address.AddressText);
                    command.Parameters.AddWithValue("@city", address.City);
                    command.Parameters.AddWithValue("@postalCode", address.PostalCode);
                    command.Parameters.AddWithValue("@country", address.Country);

                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public void DeleteAddressById(int addressId)
    {
        string query = "DELETE FROM ShippingAddresses WHERE ID = @addressId;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@addressId", addressId);

                command.ExecuteNonQuery();
            }
        }
    }
}
