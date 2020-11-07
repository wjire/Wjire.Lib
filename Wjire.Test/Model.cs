using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Wjire.Test
{
    public class Order
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? Date { get; set; }

        public List<Product> Products { get; set; }

        public Gender Gender { get; set; }

        public string Product => Products == null ? string.Empty : JsonConvert.SerializeObject(Products);
    }

    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public enum Gender
    {
        Man = 1,
        Woman = 2
    }


    public class OrderDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? Date { get; set; }

        public List<Product> Products => string.IsNullOrWhiteSpace(Product) == true ? null : JsonConvert.DeserializeObject<List<Product>>(Product);

        public Gender Gender { get; set; }

        public string Product { get; set; }
    }
}
