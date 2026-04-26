

namespace LINQ_1
{
    /// <summary>
    /// Entry point for the LINQ1 application.
    /// Demonstrates LINQ aggregate queries against a customer and sales order data set.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Iterates through all customers and displays individual order summaries,
        /// then displays aggregate statistics across all customers.
        /// </summary>
        /// <param name="args">Command-line arguments (not used).</param>
        static void Main(string[] args)
        {
            List<Customer> customers = CustomerData.Customers;
            foreach (var customer in customers)
            {
                DisplayCustomer(customer);
            }

            double overallAverage = customers
                .SelectMany(c => c.SalesOrders)
                .Average(order => order.OrderTotal);
            Customer topCustomer = customers
                .OrderByDescending(c => c.OrderTotal)
                .First();
            Console.WriteLine("--- All Customers ---");
            Console.WriteLine($"Average Order Size: {overallAverage:C}");
            Console.WriteLine($"Highest Order Total Customer: {topCustomer.Name} ({topCustomer.OrderTotal:C})");
        }

        /// <summary>
        /// Displays a summary for a single customer, including their total order amount,
        /// total backordered quantity, and average order size.
        /// </summary>
        /// <param name="customer">The <see cref="Customer"/> to display.</param>
        static void DisplayCustomer(Customer customer)
        {
            Console.WriteLine($"Customer: {customer.Name}");
            Console.WriteLine($"Total Order Amount: {customer.OrderTotal:C}");
            Console.WriteLine($"Total Backordered Quantity: {customer.BackOrdered}");
            double averageSizeOrder = customer.SalesOrders.Any()
                ? customer.SalesOrders.Average(order => order.OrderTotal)
                : 0;
            Console.WriteLine($"Average Size Order: {averageSizeOrder:C}");
            Console.WriteLine();
        }
    }
}