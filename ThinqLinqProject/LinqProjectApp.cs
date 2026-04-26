using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace ThinqLinqProject

/* ---- DO NOT MODIFY ANY CODE IN THIS FILE ---- */
{
    internal class Program
    {
        static List<Customer> customers;
        static List<Part> parts;
        static List<SalesOrder> salesOrders;
        static List<SalesOrderPart> salesOrderParts;
        static void Main(string[] args)
        {
            GetData();
            Console.WriteLine($"{Customer.StudentName}");
            TestCustomer(customers);
            Console.ReadLine();
            TestPart(parts);
            Console.ReadKey();
            TestSalesOrder(salesOrders);
            Console.WriteLine($"{Customer.StudentName}");
            Console.ReadKey();
        }
        static void TestCustomer(List<Customer> customers)
        {
            Console.WriteLine("--- Customers ---");
            foreach (var customer in customers)
                Console.WriteLine(customer.ToString());
        }
        static void TestPart(List<Part> parts)
        {
            Console.WriteLine("\n--- Parts ---");
            foreach (var part in parts)
                Console.WriteLine(part.ToString());
        }
        static void TestSalesOrder(List<SalesOrder> salesOrders)
        {
            Console.WriteLine("\n--- Sales Orders ---");
            foreach (var salesOrder in salesOrders)
                Console.WriteLine(salesOrder.ToString());
        }
        static List<Customer> GetCustomers(XDocument xmlDoc)
        {
            return xmlDoc.Descendants("Customer")
                .Select(c => new Customer
                {
                    CustomerId = (int)c.Element("CustomerId"),
                    FirstName = (string)c.Element("FirstName"),
                    LastName = (string)c.Element("LastName"),
                    City = (string)c.Element("City"),
                    State = (string)c.Element("State")
                }).ToList();
        }
        static List<Part> GetParts(XDocument xmlDoc)
        {
            return xmlDoc.Descendants("Part")
                .Select(c => new Part
                {
                    PartId = (int)c.Element("PartId"),
                    Name = (string)c.Element("Name"),
                    Price = decimal.Parse((string)c.Element("Price")),
                    ReceivedUnits = int.Parse((string)c.Element("ReceivedUnits")),
                    ShippedUnits = int.Parse((string)c.Element("ShippedUnits")),
                    SpoiledUnits = int.Parse((string)c.Element("SpoiledUnits")),
                    ReceivedValue = decimal.Parse((string)c.Element("ReceivedValue")),
                    ShippedValue = decimal.Parse((string)c.Element("ShippedValue")),
                    SpoiledValue = decimal.Parse((string)c.Element("SpoiledValue"))
                }).ToList();
        }
        static List<SalesOrder> GetSalesOrders(XDocument xmlDoc)
        {
            return xmlDoc.Descendants("SalesOrder")
                .Select(c => new SalesOrder
                {
                    SalesOrderNumber = (int)c.Element("SalesOrderNumber"),
                    CustomerId = (int)c.Element("CustomerId"),
                    OrderDate = DateTime.Parse((string)c.Element("OrderDate"))
                }).ToList();
        }
        static List<SalesOrderPart> GetSalesOrderParts(XDocument xmlDoc)
        {
            return xmlDoc.Descendants("SalesOrderPart")
                .Select(c => new SalesOrderPart
                {
                    SalesOrderNumber = (int)c.Element("SalesOrderNumber"),
                    PartId = (int)c.Element("PartId"),
                    Quantity = (int)c.Element("Quantity"),
                    UnitsShipped = (int)c.Element("UnitsShipped"),
                    UnitPrice = (decimal)c.Element("UnitPrice"),
                    UnitCost = (decimal)c.Element("UnitCost")
                }).ToList();
        }
        static void GetData()
        {
            XDocument xmlDoc = XDocument.Load("../../../LinqProject.xml");
            customers = GetCustomers(xmlDoc);
            parts = GetParts(xmlDoc);
            salesOrders = GetSalesOrders(xmlDoc);
            salesOrderParts = GetSalesOrderParts(xmlDoc);
            var salesOrdersLookup = salesOrders.ToLookup(so => so.CustomerId);
            var salesOrderPartsLookup = salesOrderParts.ToLookup(sop => sop.SalesOrderNumber);
            foreach (var customer in customers)
            {
                customer.SalesOrders = salesOrders.Where(so => so.CustomerId == customer.CustomerId).ToList();
                //customer.SalesOrders = salesOrdersLookup[customer.CustomerId].ToList();
            }
            foreach (var salesOrder in salesOrders)
            {
                salesOrder.SalesOrderParts = salesOrderPartsLookup[salesOrder.SalesOrderNumber].ToList();
                salesOrder.Customer = customers.First(c => c.CustomerId == salesOrder.CustomerId);
            }
            foreach (var part in parts)
            {
                part.SalesOrderParts = salesOrderParts.Where(sop => sop.PartId == part.PartId).ToList();
            }
            foreach (var sop in salesOrderParts)
            {
                sop.Part = parts.First(p => p.PartId == sop.PartId);
                sop.SalesOrder = salesOrders.First(so => so.SalesOrderNumber == sop.SalesOrderNumber);
            }
        }
    }
}
/* ---- DO NOT MODIFY ANY CODE IN THIS FILE ---- */