﻿using Case5.Infra.Entities;
using DapperExtensions.Mapper;

namespace Case5.Infra.Dapper
{
    public class CustomerMapper : ClassMapper<Customer>
    {
        public CustomerMapper()
        {
            Table("Customers");

            Map(x => x.Id).Key(KeyType.Guid);

            AutoMap();
        }
    }
}