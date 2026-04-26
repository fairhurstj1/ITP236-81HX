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
        public const string StudentName = "Put your name here";             //--< START HERE WITH YOUR NAME <<<

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
        /// LargestSale is the largest sale for the Customer based on OrderTotal
        /// </summary>
        public SalesOrder LargestSale => new SalesOrder();
        /// <summary>
        /// Returns a collection (List) of the items that a Customer has purchased, with the total quantities
        /// Group the SalesOrderParts from the SalesOrders. Group by the Part's PartId and Name
        /// For each group, 
        ///     create a new CustomerItem object summing the 
        ///     Quantites, ExtendedPrices, UnitsShipped and 
        ///     the differences between Quantities and UnitsShipped for the Backorder
        /// </summary>

        ///List of CustomerItems is the list of items that a Customer has purchased, with the total quantities
        public List<CustomerItem> CustomerItems => new List<CustomerItem>();
        
    }

    public partial class Part
    {
        #region Quantities
        /// <summary>
        /// QuantityOnHand = Units Received - Units Spoiled - Units Shipped
        /// </summary>
        public int QuantityOnHand => 0;

        /// <summary>
        /// UnitsSold is the sum of the sales for the Part. Use SalesOrderParts.
        /// </summary>
        public int UnitsSold => 0;

        #endregion
        #region Amounts
        /// <summary>
        /// CurrentValue =  Received Value - Spoiled Value -  Shipped Value
        /// </summary>
        public decimal CurrentValue => 0;
        /// Amount Sold is the sum of the extended prices for the SalesOrderParts.
        /// </summary>
        public decimal AmountSold => 0;

        #endregion
        /// <summary>
        /// Customers is the list of Customers that have purchased this part from us.
        /// Start with Sales Order Parts, find the Sales Order for each one
        ///     Then get the Customers for the Sales Orders.
        /// We only want one distinct object for each customer.
        /// Create a List of the Customers.
        /// </summary>

        ///List of Customers is the list of Customers that have purchased this part from us.
        public List<Customer> Customers => new List<Customer>();
    }

    public partial class SalesOrder
    {
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