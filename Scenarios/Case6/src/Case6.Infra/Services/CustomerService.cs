﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Tnf.Configuration;

namespace Case6.Infra.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DbProviderFactory providerFactory;

        public CustomerService(DbProviderFactory providerFactory)
        {
            this.providerFactory = providerFactory;
        }

        public IEnumerable<CustomerDto> GetAllCustomers()
        {
            var customers = new List<CustomerDto>();

            using (var connection = providerFactory.CreateConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT * FROM CustomerFromADO";

                    using (var reader = command.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        while (reader.Read())
                        {
                            customers.Add(new CustomerDto()
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString()
                            });
                        }
                    }
                }
            }

            return customers;
        }
    }
}
