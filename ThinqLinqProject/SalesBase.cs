using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* ---- DO NOT MODIFY ANY CODE IN THIS FILE ---- */

namespace ThinqLinqProject
{
    /*   Poor Man's ERD
    +----------------+        +------------------+        +----------------------+        +----------------+
    |    Customer    | 1    * |   SalesOrder     | 1    * |   SalesOrderPart     | *    1 |      Part      |
    +----------------+--------+------------------+--------+----------------------+--------+----------------+
    | CustomerId (PK)|        | SalesOrderId (PK)|        | SalesOrderPartId (PK)|        | PartId (PK)    |
    | Name           |        | CustomerId (FK)  |        | SalesOrderId (FK)    |        | PartName       |
    | Email          |        | OrderDate        |        | PartId (FK)          |        | Description    |
    | Phone          |        | TotalAmount      |        | Quantity             |        | UnitPrice      |
    +----------------+        | Status           |        | UnitPriceAtSale      |        +----------------+
                              +------------------+        +----------------------+
    */
    public partial class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public List<SalesOrder> SalesOrders { get; set; }

        public Customer() : this(-1, string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        public Customer(int customerId, string firstName, string lastName, string city, string state)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            City = city;
            State = state;
            SalesOrders = new List<SalesOrder>();
        }
        public override string ToString()
        {
            var toString = new StringBuilder();
            toString.Append(
                $"Customer: {FirstName} {LastName}\n" +
                //$"\tTotal Cost: {TotalCost.ToString("c")}\n" +
                //$"\tGross Profit {GrossProfit.ToString("c")}\n" +
                $"\tItems Sold {ItemsSold}\n" +
                $"\tLargest Sale: {(LargestSale?.OrderTotal.ToString("c") ?? "0")}\n" +
                $"\tCustomer Items Count: {CustomerItems.Count()}\n" +
                $"\t\tCustomer Items\n");
            foreach (var custItem in CustomerItems)
            {
                toString.Append(custItem.ToString());
            }
            return toString.ToString();
        }
    }
    public partial class Part
    {
        public int PartId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal ReceivedValue { get; set; }
        public decimal ShippedValue { get; set; }
        public int SpoiledUnits { get; set; }
        public int ReceivedUnits { get; set; }
        public int ShippedUnits { get; set; }
        public decimal SpoiledValue { get; set; }
        public List<SalesOrderPart> SalesOrderParts { get; set; }
        public Part() : this(0, string.Empty, 0m) { }
        public Part(int partId, string name, decimal price)
        {
            PartId = partId;
            Name = name;
            Price = price;
            SalesOrderParts = new List<SalesOrderPart>();
        }
        public override string ToString()
        {
            return $"PartId: {PartId}\n" +
                $"Part: {Name}\n" +
                $"\tQoH: {QuantityOnHand}\n" +
                $"\tUnits Sold {UnitsSold}\n" +
                $"\tInventory Value {CurrentValue.ToString("c")}\n" +
                $"\tAmount Sold: {AmountSold.ToString("c")} \n" +
                $"\tCustomers: {Customers.Count()}";
        }
    }

    public partial class SalesOrder
    {
        public int SalesOrderNumber { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }

        #region Sales Status

        enum StatusType
        {
            Open = 1,
            Closed
        }

        public int SalesStatusId { get; set; }

        public string SalesStatus
        {
            get
            {
                var status = (StatusType)SalesStatusId;
                return status.ToString();
            }
        }

        #endregion

        public List<SalesOrderPart> SalesOrderParts { get; set; }

        public SalesOrder() : this(-1, -1, DateTime.Now)
        {

        }

        public SalesOrder(int salesOrderNumber, int customerId, DateTime orderDate)
        {
            SalesOrderNumber = salesOrderNumber;
            CustomerId = customerId;
            OrderDate = orderDate;
            SalesOrderParts = new List<SalesOrderPart>();
        }
        public override string ToString()
        {
            return $"SalesOrder: {SalesOrderNumber}\n" +
                $"\t{Customer.FirstName} {Customer.LastName}\n" +
                $"\tItems Sold: {ItemsSold}\n" +
                $"\tUnits Shipped {UnitsShipped}\n" +
                $"\tBackOrdered {BackOrdered}\n" +
                $"\tOrder Total: {OrderTotal.ToString("c")}\n" +
                $"\tOrder Cost: {OrderCost.ToString("c")}\n" +
                $"\tGross Profit: {GrossProfit}";
        }
    }

    public partial class SalesOrderPart
    {
        public int SalesOrderPartId { get; set; }
        public int SalesOrderNumber { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public int PartId { get; set; }
        public Part Part { get; set; }
        public int Quantity { get; set; }
        public int UnitsShipped { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }
        public SalesOrderPart() : this(-1, -1, -1, 0, 0m, 0m)
        { }
        public SalesOrderPart(int salesOrderPartId, int salesOrderNumber,
            int partId, int quantity, decimal price, decimal unitCost)
        {
            SalesOrderPartId = salesOrderPartId;
            SalesOrderNumber = salesOrderNumber;
            PartId = partId;
            Quantity = quantity;
            UnitPrice = price;
            UnitCost = unitCost;
        }
    }

    public partial class CustomerItem
    {
        public int CustomerId { get; set; }
        public int PartId { get; set; }
        public string Part { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitPrice => Quantity == 0 ? 0 : Amount / Quantity;
        public int Shipped { get; set; }
        public int BackOrdered { get; set; }
        public CustomerItem() { }
        public CustomerItem(int customerId, int partId, string part, int quantity, decimal amount, int shipped,
            int backOrdered)
        {
            CustomerId = customerId;
            PartId = partId;
            Part = part;
            Quantity = quantity;
            Amount = amount;
            Shipped = shipped;
            BackOrdered = backOrdered;
        }
        public override string ToString() {
            return $"\t\t------------------\n" +
                $"\t\tCustomerId: {CustomerId}\n" +
                $"\t\tPartId: {PartId}\n" +
                $"\t\tQuantity: {Quantity}\n" +
                $"\t\tAmount: {Amount.ToString("c")}\n" +
                $"\t\tShipped: {Shipped}\n" +
                $"\t\tBack Ordered: {BackOrdered}\n";
        }
    }
}

/* ---- DO NOT MODIFY ANY CODE IN THIS FILE ---- */