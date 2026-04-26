using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ_1
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Region { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        //--< Rewrite this to get the total order from the SalesOrders' OrderTotals using LINQ <<<
        public double OrderTotal
        {
            get
            {
                return SalesOrders.Sum(so => so.OrderTotal);
            }
        }

        //--< Rewrite this to get the total quantity backordered (Quantity - Shipped)   <<<
        public int BackOrdered
        {
            get
            {
                return SalesOrders.Sum(so => so.Quantity - so.Shipped);
            }
        }
        public List<SalesOrder> SalesOrders { get; set; }
        public Customer()
        {
            SalesOrders = new List<SalesOrder>();
        }
    }
    public class SalesOrder
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public double OrderTotal { get; set; }
        public int Quantity { get; set; }
        public int Shipped { get; set; }
    }
    public static class CustomerData
    {
        public static List<Customer> Customers
        {
            get
            {
                return new List<Customer>()
                {
                    new Customer() {
                        CustomerId = 1,
                        Name = "Skip Wythe",
                        Region = "Central Virginia",
                        City = "Richmond",
                        State = "VA",
                        SalesOrders = new List<SalesOrder>()
                        {
                            new SalesOrder() {
                                OrderId = 101,
                                CustomerId = 1,
                                OrderDate = new DateTime(2025,01,02),
                                OrderTotal = 125,
                                Quantity = 5,
                                Shipped = 5
                            },
                            new SalesOrder() {
                                OrderId = 102,
                                CustomerId = 1,
                                OrderDate = new DateTime(2025,01,05),
                                OrderTotal = 2125,
                                Quantity = 17,
                                Shipped = 17
                            },
                            new SalesOrder() {
                                OrderId = 105,
                                CustomerId = 1,
                                OrderDate = new DateTime(2025,02,12),
                                OrderTotal = 1133,
                                Quantity = 21,
                                Shipped = 21
                            }
                        }
                    },
                    new Customer() {
                        CustomerId = 2,
                        Name = "James River",
                        Region = "Tidewater",
                        City = "Williamsburg",
                        State = "VA",
                        SalesOrders = new List<SalesOrder>()
                        {
                            new SalesOrder() {
                                OrderId = 103,
                                CustomerId = 2,
                                OrderDate = new DateTime(2024,12,14),
                                OrderTotal = 377,
                                Quantity = 15,
                                Shipped = 13
                            },
                            new SalesOrder() {
                                OrderId = 104,
                                CustomerId = 2,
                                OrderDate = new DateTime(2024,01,07),
                                OrderTotal = 1833,
                                Quantity = 14,
                                Shipped = 14
                            },
                            new SalesOrder() {
                                OrderId = 107,
                                CustomerId = 2,
                                OrderDate = new DateTime(2025,02,11),
                                OrderTotal = 2024,
                                Quantity = 31,
                                Shipped = 23
                            },
                            new SalesOrder() {
                                OrderId = 109,
                                CustomerId = 2,
                                OrderDate = new DateTime(2025,02,11),
                                OrderTotal = 3480,
                                Quantity = 13,
                                Shipped = 11
                            }
                        }
                    },
                    new Customer() {
                        CustomerId = 3,
                        Name = "Maggie Walker",
                        Region = "Tidewater",
                        City = "Williamsburg",
                        State = "VA",
                        SalesOrders = new List<SalesOrder>()
                        {
                            new SalesOrder() {
                                OrderId = 108,
                                CustomerId = 3,
                                OrderDate = new DateTime(2024,12,11),
                                OrderTotal = 1830,
                                Quantity = 10,
                                Shipped = 10
                            },
                            new SalesOrder() {
                                OrderId = 111,
                                CustomerId = 3,
                                OrderDate = new DateTime(2025,02,15),
                                OrderTotal = 4130,
                                Quantity = 38,
                                Shipped = 0
                            }
                        }
                    }
                };
            }
        }
    }
}
