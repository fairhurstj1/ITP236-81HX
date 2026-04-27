using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_Net
{
    /* ---- DO NOT MODIFY ANY CODE IN THIS FILE ---- */
    public partial class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        /// <summary>
        /// TotalSales is the sum of the SalesOrders' OrderTotal
        /// </summary>
        public decimal TotalSales => 0;
        /// <summary>
        /// TotalCost is the sum of the SalesOrders' OrderCost
        /// </summary>
        public decimal TotalCost => 0;
        /// <summary>
        /// GrossProfit is the difference between TotalSales and TotalCost
        /// </summary>
        public decimal GrossProfit => 0;
        /// <summary>
        /// ItemsSold is the sum of the SalesOrders' SalesOrderParts Quantities
        /// </summary>
        public int ItemsSold => 0;
        /// <summary>
        /// Returns a collection (List) of the items that a Customer has purchased, with the total quantities
        /// Group the SalesOrderParts from the SalesOrders. Group by the Part's PartId and Name
        /// For each group, 
        ///     create a new CustomerItem object summing the 
        ///     Quantites, ExtendedPrices, UnitsShipped and 
        ///     the differences between Quantities and UnitsShipped for the Backorder
        /// </summary>

        ///List of CustomerItems is the list of items that a Customer has purchased, with the total quantities
        public List<CustomerItem> CustomerItems => (from so in SalesOrders
                                                    from sop in so.SalesOrderParts
                                                    group sop by new
                                                    {
                                                        sop.PartId,
                                                        sop.Part.Name
                                                    }
                                                    into sopGroup
                                                    select new CustomerItem()
                                                    {
                                                        CustomerId = CustomerId,
                                                        PartId = sopGroup.Key.PartId,
                                                        Part = sopGroup.Key.Name,
                                                        Amount = sopGroup.Sum(sog => sog.ExtendedPrice),
                                                        Shipped = sopGroup.Sum(sog => sog.UnitsShipped),
                                                        Quantity = sopGroup.Sum(sog => sog.Quantity),
                                                        BackOrdered = sopGroup.Sum(sog => sog.Quantity - sog.UnitsShipped)
                                                    }).ToList();
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
                //$"\tLargest Sale: {(LargestSale?.OrderTotal.ToString("c") ?? "0")}\n" +
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
        /// <summary>
        /// Customers is the list of Customers that have purchased this part from us.
        /// Start with Sales Order Parts, find the Sales Order for each one
        ///     Then get the Customers for the Sales Orders.
        /// We only want one distinct object for each customer.
        /// Create a List of the Customers.
        /// </summary>
        public List<Customer> Customers => new List<Customer>();
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
                //$"\tQoH: {QuantityOnHand}\n" +
                //$"\tUnits Sold {UnitsSold}\n" +
                //$"\tInventory Value {CurrentValue.ToString("c")}\n" +
                //$"\tAmount Sold: {AmountSold.ToString("c")} \n" +
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
        #region Quantities
        /// <summary>
        /// ItemsSold is the sum of the quantities for SalesOrderParts
        /// </summary>
        public int ItemsSold => 0;

        /// <summary>
        /// ItemsShipped is the sum of the SalesOrderParts UnitsShipped Quantities
        /// </summary>
        public int UnitsShipped => 0;
        /// <summary>
        /// BackOrdered is the difference between the Items Sold and the Items Shipped
        /// </summary>
        public int BackOrdered => 0;
        #endregion

        #region Amounts
        /// <summary>
        /// OrderTotal is the sum of the SalesOrderParts' Extended Prices
        /// </summary>
        public decimal OrderTotal => 0;
        /// <summary>
        /// OrderCost is the sum of the SalesOrderPart's Extended Costs
        /// </summary>
        decimal OrderCost => 0;
        /// <summary>
        /// GrossProfit is the difference between the Order Total and the Order Cost
        /// </summary>
        decimal GrossProfit => 0;

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
        public decimal ExtendedPrice => Quantity * UnitPrice;
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

    public class ProjectData
    {
        public List<Customer> Customers { get; set; }
        public List<Part> Parts { get; set; }
        public List<SalesOrder> SalesOrders { get; set; }
        public List<SalesOrderPart> SalesOrderParts { get; set; }
    }
}