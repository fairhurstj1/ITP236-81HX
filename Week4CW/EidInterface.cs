using System;
using System.Collections.Generic;
namespace Week4CW
{
    /// <summary>
    /// Defines shared business associate data.
    /// </summary>
    public interface IBusinessAssociate
    {
        /// <summary>
        /// Gets or sets the name of the business associate.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Gets the unique identifier for the business associate.
        /// </summary>
        int Id { get; }
        /// <summary>
        /// Gets or sets the total amount of business transactions associated with this business associate.
        /// </summary>
        double TotalAmount { get; set; }
    }

    /// <summary>
    /// Represents a base customer record.
    /// </summary>
    public abstract class Customer {
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Represents a consumer business associate.
    /// </summary>
    public class Consumer : Customer, IBusinessAssociate
    {
        /// <summary>
        /// Gets the consumer identifier.
        /// </summary>
        public int ConsumerId => CustomerId;

        /// <summary>
        /// Gets the business associate identifier.
        /// </summary>
        public int Id => ConsumerId;

        /// <summary>
        /// Gets or sets the total amount associated with the consumer.
        /// </summary>
        public double TotalAmount { get; set; }
    }

    /// <summary>
    /// Represents a vendor business associate.
    /// </summary>
    public class Vendor : Customer, IBusinessAssociate
    {
        /// <summary>
        /// Gets the vendor identifier.
        /// </summary>
        public int VendorId => CustomerId;

        /// <summary>
        /// Gets the business associate identifier.
        /// </summary>
        public int Id => VendorId;

        /// <summary>
        /// Gets or sets the total amount associated with the vendor.
        /// </summary>
        public double TotalAmount { get; set; }
    }
}