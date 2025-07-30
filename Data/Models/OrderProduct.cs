﻿using Data.Models;

namespace Data.Models
{
    public class OrderProduct
    {
        public Guid OrderId { get; set; }
        public required Order Order { get; set; }

        public Guid ProductId { get; set; }
        public required Product Product { get; set; }
    }

}