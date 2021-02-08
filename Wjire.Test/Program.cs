using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSRedis;
using Newtonsoft.Json;

namespace Wjire.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var order = new Order
            {
                Products = new List<Product>
                {
                    new Product {Id = 1, Name = "p1"},
                    new Product {Id = 2, Name = "p2"},
                },
                Name = "order",
                Id = 1,
                Gender = Gender.Man,
                Date = DateTime.Now,
            };


            var dto = order.ExpressionDeepCopyTo<Order, OrderDto>();
            Console.WriteLine(JsonConvert.SerializeObject(order));
            Console.WriteLine();
            Console.WriteLine(JsonConvert.SerializeObject(dto));
            Console.WriteLine("===========================================");
            order.Products.FirstOrDefault().Name = "ppppp";
            dto = new OrderDto();
            order.ExpressionCopyPropertyTo(dto);
            Console.WriteLine(JsonConvert.SerializeObject(order));
            Console.WriteLine();
            Console.WriteLine(JsonConvert.SerializeObject(dto));



            //{
            //    var newOrder = ExpressionCopy<Order>.DeepCopyFrom(order);
            //    Console.WriteLine(object.ReferenceEquals(order, newOrder));

            //    Console.WriteLine(JsonConvert.SerializeObject(order));
            //    Console.WriteLine();
            //    Console.WriteLine(JsonConvert.SerializeObject(newOrder));

            //    Console.WriteLine("===========================================");

            //    order.Id = 3;
            //    order.Name = "refuge";
            //    order.Products.FirstOrDefault().Name = "pppp";
            //    Console.WriteLine(JsonConvert.SerializeObject(order));
            //    Console.WriteLine();
            //    Console.WriteLine(JsonConvert.SerializeObject(newOrder));


            //    Console.WriteLine("===========================================");
            //    Console.WriteLine();

            //    //Action<Order, Order> action = (source, target) =>
            //    // {
            //    //     target.Id = source.Id;
            //    //     target.Name = source.Name;
            //    // };
            //    //action(order, newOrder);
            //    //Console.WriteLine(order.Id + "_" + order.Name);
            //    //Console.WriteLine(newOrder.Id + "_" + newOrder.Name);

            //    ExpressionCopyProperties<Order, Order>.DeepCopyFrom(order, newOrder);
            //    Console.WriteLine();
            //    Console.WriteLine(JsonConvert.SerializeObject(order));
            //    Console.WriteLine();
            //    Console.WriteLine(JsonConvert.SerializeObject(newOrder));
            //}

            Console.ReadKey();
        }
    }
}
