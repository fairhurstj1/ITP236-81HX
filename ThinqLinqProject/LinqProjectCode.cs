using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinqLinqProject
{
    /* ---- ALL OF YOUR CODE FOR THE THINQ LINQ PROJECT GOES IN THIS FILE ---- */
    public partial class Customer
    {
        public const string StudentName = "Jonathan Fairhurst";             //--< START HERE WITH YOUR NAME <<<

        /// <summary>
        /// TotalSales is the sum of the SalesOrders' OrderTotal
        /// </summary>
        public decimal TotalSales => SalesOrders.Sum(so => so.OrderTotal); 
        /// <summary>
        /// TotalCost is the sum of the SalesOrders' OrderCost
        /// </summary>
        public decimal TotalCost => SalesOrders
            .SelectMany(so => so.SalesOrderParts)
            .Sum(sop => sop.ExtendedCost);
        /// <summary>
        /// GrossProfit is the difference between TotalSales and TotalCost
        /// </summary>
        public decimal GrossProfit => 
            SalesOrders
                .SelectMany(so => so.SalesOrderParts)
                .Sum(sop => sop.ExtendedPrice - sop.ExtendedCost);

        /// <summary>
        /// ItemsSold is the sum of the SalesOrders' SalesOrderParts Quantities
        /// </summary>
        public int ItemsSold => 
            SalesOrders
                .SelectMany(so => so.SalesOrderParts)
                .Sum(sop => sop.Quantity);

        /// <summary>
        /// LargestSale is the largest sale for the Customer based on OrderTotal
        /// </summary>
        public SalesOrder LargestSale => 
            SalesOrders
                .MaxBy(so => so.OrderTotal)
                 ?? new SalesOrder();
        /// <summary>
        /// Returns a collection (List) of the items that a Customer has purchased, with the total quantities
        /// Group the SalesOrderParts from the SalesOrders. Group by the Part's PartId and Name
        /// For each group, 
        ///     create a new CustomerItem object summing the 
        ///     Quantites, ExtendedPrices, UnitsShipped and 
        ///     the differences between Quantities and UnitsShipped for the Backorder
        /// </summary>

        ///List of CustomerItems is the list of items that a Customer has purchased, with the total quantities
        public List<CustomerItem> CustomerItems => SalesOrders
            .SelectMany(so => so.SalesOrderParts)
            .GroupBy(sop => new { sop.Part.PartId, sop.Part.Name })
            .Select(g => new CustomerItem
            {
                Quantity = g.Sum(sop => sop.Quantity),
                Amount = g.Sum(sop => sop.ExtendedPrice),
                Shipped = g.Sum(sop => sop.UnitsShipped),
                BackOrdered = g.Sum(sop => sop.Quantity - sop.UnitsShipped),
                PartId = g.Key.PartId,
                Part = g.Key.Name,
                CustomerId = this.CustomerId
            }).ToList();

                
        
    }

    public partial class Part
    {
        #region Quantities
        /// <summary>
        /// QuantityOnHand = Units Received - Units Spoiled - Units Shipped
        /// </summary>
        public int QuantityOnHand => 
            new[] { ReceivedUnits, -SpoiledUnits, -ShippedUnits }.Sum();

        /// <summary>
        /// UnitsSold is the sum of the sales for the Part. Use SalesOrderParts.
        /// </summary>
        public int UnitsSold => SalesOrderParts.Sum(sop => sop.Quantity);

        #endregion
        #region Amounts
        /// <summary>
        /// CurrentValue =  Received Value - Spoiled Value -  Shipped Value
        /// </summary>
        public decimal CurrentValue => new[] 
            { ReceivedValue, -SpoiledValue, -ShippedValue }
            .Sum();
        /// <summary>
        /// Amount Sold is the sum of the extended prices for the SalesOrderParts.
        /// </summary>
        public decimal AmountSold => SalesOrderParts.Sum(sop => sop.ExtendedPrice);

        #endregion
        /// <summary>
        /// Customers is the list of Customers that have purchased this part from us.
        /// Start with Sales Order Parts, find the Sales Order for each one
        ///     Then get the Customers for the Sales Orders.
        /// We only want one distinct object for each customer.
        /// Create a List of the Customers.
        /// </summary>

        ///List of Customers is the list of Customers that have purchased this part from us.
        public List<Customer> Customers => 
            SalesOrderParts
                .Select(sop => sop.SalesOrder.Customer)
                .DistinctBy(c => c.CustomerId)
                .ToList();
    }

    public partial class SalesOrder
    {
        #region Quantities
        /// <summary>
        /// ItemsSold is the sum of the quantities for SalesOrderParts
        /// </summary>
        public int ItemsSold => SalesOrderParts.Sum(sop => sop.Quantity);

        /// <summary>
        /// ItemsShipped is the sum of the SalesOrderParts UnitsShipped Quantities
        /// </summary>
        public int UnitsShipped => SalesOrderParts.Sum(sop => sop.UnitsShipped);
        /// <summary>
        /// BackOrdered is the difference between the Items Sold and the Items Shipped
        /// </summary>
        public int BackOrdered => SalesOrderParts.Sum(sop => sop.Quantity - sop.UnitsShipped);
        #endregion

        #region Amounts
        /// <summary>
        /// OrderTotal is the sum of the SalesOrderParts' Extended Prices
        /// </summary>
        public decimal OrderTotal => SalesOrderParts.Sum(sop => sop.ExtendedPrice);
        /// <summary>
        /// OrderCost is the sum of the SalesOrderPart's Extended Costs
        /// </summary>
        decimal OrderCost => SalesOrderParts.Sum(sop => sop.ExtendedCost);
        /// <summary>
        /// GrossProfit is the difference between the Order Total and the Order Cost
        /// </summary>
        decimal GrossProfit => SalesOrderParts.Sum(sop => sop.ExtendedPrice - sop.ExtendedCost);
        
        #endregion
    }

    /// <summary>
    /// Represents a component of a sales order, providing calculated pricing and cost information for the part.
    /// </summary>
    public partial class SalesOrderPart
    {
        public decimal ExtendedPrice => Quantity * UnitPrice;
        public decimal ExtendedCost => Quantity * UnitCost;
    }
}
/* ---- ALL OF YOUR CODE FOR THE THINQ LINQ PROJECT GOES IN THIS FILE ---- */