﻿using System;
using System.Collections.Generic;

namespace Querying.Infra.Dto
{
    public class SumarizedPurchaseOrder
    {
        public DateTime Date { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
        public IEnumerable<SumarizedProduct> Products { get; set; }
    }

    public class SumarizedProduct
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public decimal TotalValue { get; set; }
    }
}
