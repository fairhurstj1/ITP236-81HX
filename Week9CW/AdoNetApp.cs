using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Configuration;
using System.Text.Json;



namespace ADO_Net
{
    internal class Program
    {
        static List<Customer> customers;
        static List<Part> parts;
        static List<SalesOrder> salesOrders;
        static List<SalesOrderPart> salesOrderParts;
        static string connectionString = GetConnectionString();
        static string xmlData = ResolveDataFilePath(ConfigurationManager.AppSettings["xmlData"]);
        static string xmlNewData = ResolveDataFilePath(ConfigurationManager.AppSettings["xmlNewData"]);
        static string jsonData = ResolveDataFilePath(ConfigurationManager.AppSettings["jsonData"]);

        static string GetConnectionString()
        {
            const string envVarName = "WEEK9CW_CONNECTION_STRING";
            string connectionStringFromEnv = Environment.GetEnvironmentVariable(envVarName);
            if (!string.IsNullOrWhiteSpace(connectionStringFromEnv))
            {
                return connectionStringFromEnv;
            }

            string connectionStringFromConfig = ConfigurationManager.AppSettings["connectionString"];
            if (!string.IsNullOrWhiteSpace(connectionStringFromConfig) &&
                !connectionStringFromConfig.Contains("REPLACE_WITH", StringComparison.OrdinalIgnoreCase))
            {
                return connectionStringFromConfig;
            }

            throw new InvalidOperationException(
                $"No usable connection string found. Set {envVarName} in your environment or update appSettings['connectionString'] locally.");
        }

        static string ResolveDataFilePath(string configuredPath)
        {
            if (string.IsNullOrWhiteSpace(configuredPath))
            {
                throw new InvalidOperationException("Missing required appSettings path value.");
            }

            if (Path.IsPathRooted(configuredPath) && File.Exists(configuredPath))
            {
                return configuredPath;
            }

            var candidatePaths = new List<string>
            {
                Path.GetFullPath(configuredPath, Directory.GetCurrentDirectory()),
                Path.GetFullPath(configuredPath, AppContext.BaseDirectory)
            };

            string fileName = Path.GetFileName(configuredPath);
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null)
            {
                candidatePaths.Add(Path.Combine(directory.FullName, fileName));
                directory = directory.Parent;
            }

            string resolvedPath = candidatePaths
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault(File.Exists);

            if (resolvedPath != null)
            {
                return resolvedPath;
            }

            throw new FileNotFoundException(
                $"Could not locate configured data file '{configuredPath}'. Checked: {string.Join(", ", candidatePaths.Distinct(StringComparer.OrdinalIgnoreCase))}");
        }

        static void Main(string[] args)
        {
            GetXmlData(); 
            CreateXml();
            GetJsonData();
            AdoNet();
            CleanupData();
            LoadCustomers();
            LoadSalesOrders();
            LoadParts();
            LoadSalesOrderParts();
                ReadCustomers();

            //var dbCustomers = GetCustomersFromDb();
            //foreach (var cust in dbCustomers)
            //{
                //Console.WriteLine($"{cust.FirstName} {cust.LastName}\thas {cust.SalesOrders.Count} orders");
            //}
        }
        static List<Customer> GetCustomersFromDb()
        {
            //using (var db = new ITP236_13Entities())
            //{
            //    var customers = db.Customers
            //        .Include(c => c.SalesOrders);
            //    var sql = customers.ToString();
            //    return customers.ToList();
            //}
            return new List<Customer>();
        }
        static void CleanupData()
        {
            string cleanupData = @"
                DELETE FROM Sales.SalesOrderPart; 
                DELETE FROM Sales.SalesOrder; 
                DELETE FROM Sales.Part; 
                DELETE FROM Sales.Customer;
            ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open the database connection
                    using (SqlCommand command = new SqlCommand(cleanupData, connection))
                    {
                        //--< Out with the old data >--//
                        int rowsAffected = command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Cleanup Failed. Reason: {ex.Message}");
                }
            }
        }
        static void ReadCustomers()
        {
            string query = "SELECT CustomerId, FirstName, LastName, City, State FROM Sales.Customer";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    // Create a SqlCommand to execute the query
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Execute the query and get a SqlDataReader object
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Check if there are rows to read
                            if (reader.HasRows)
                            {
                                Console.WriteLine("Customer Data:");
                                Console.WriteLine("---------------");

                                // Read each row
                                while (reader.Read())
                                {
                                    // Retrieve data by column names or indexes
                                    int customerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                                    string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                                    string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                                    string city = reader.GetString(reader.GetOrdinal("City"));
                                    string state = reader.GetString(reader.GetOrdinal("State"));

                                    var customer = new Customer
                                    {
                                        CustomerId = customerId,
                                        FirstName = firstName,
                                        LastName = lastName,
                                        City = city,
                                        State = state
                                    };
                                    Console.WriteLine(customer.ToString());

                                    // Display the data
                                    //Console.WriteLine($"CustomerId: {customerId}, Name: {firstName} {lastName}, City: {city}, State: {state}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No data found in the Customers table.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., connection issues, SQL errors)
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
        static void LoadCustomers()
        {
            string enableIdentityInsert = "SET IDENTITY_INSERT [Sales].[Customer] ON";

            var insertCustomer = @"
                INSERT INTO Sales.Customer(CustomerId, FirstName, LastName, 
                City, State, CustomerType)
                VALUES (@CustomerId, @FirstName, @LastName, @City, @State, @CustomerType)
            ";
            XDocument xmlDoc = XDocument.Load(xmlData);
            var customers = xmlDoc.Descendants("Customer");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open the database connection
                    using (SqlCommand command = new SqlCommand(enableIdentityInsert, connection))
                    {
                        //--< Turn on Identity Insert >--//
                        command.ExecuteNonQuery();

                        //--< In with the new data >--//
                        command.CommandText = insertCustomer;
                        // Add parameters to avoid SQL injection
                        foreach (var customer in customers)
                        {
                            string customerId = customer.Element("CustomerId").Value;
                            string firstName = customer.Element("FirstName").Value;
                            string lastName = customer.Element("LastName").Value;
                            string city = customer.Element("City").Value;
                            string state = customer.Element("State").Value;
                            string customerType = customer.Element("CustomerType").Value;

                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@CustomerId", customerId);
                            command.Parameters.AddWithValue("@FirstName", firstName);
                            command.Parameters.AddWithValue("@LastName", lastName);
                            command.Parameters.AddWithValue("@City", city);
                            command.Parameters.AddWithValue("@State", state);
                            command.Parameters.AddWithValue("@CustomerType", customerType);
                            int rowsAffected = command.ExecuteNonQuery();
                            Console.WriteLine($"{firstName} {lastName} inserted");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        string disableIdentityInsert = "SET IDENTITY_INSERT [Sales].[Customer] OFF";
                        using (SqlCommand command = new SqlCommand(disableIdentityInsert, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
            }
        }
        static void LoadSalesOrders()
        {
            string enableIdentityInsert = "SET IDENTITY_INSERT [Sales].[SalesOrder] ON";

            var insertSalesOrder = "INSERT INTO Sales.SalesOrder(SalesOrderNumber, CustomerId, OrderDate) " +
                "VALUES (@SalesOrderNumber, @CustomerId, @OrderDate)";
            XDocument xmlDoc = XDocument.Load(xmlData);
            var salesOrders = xmlDoc.Descendants("SalesOrder");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open the database connection
                    using (SqlCommand command = new SqlCommand(enableIdentityInsert, connection))
                    {
                        //--< Turn Identity Insert On so that we can input PKs >--//
                        command.ExecuteNonQuery();

                        //--< In with the new data >--//
                        command.CommandText = insertSalesOrder;
                        // Add parameters to avoid SQL injection
                        foreach (var so in salesOrders)
                        {
                            string salesOrderNumber = so.Element("SalesOrderNumber").Value;
                            string customerId = so.Element("CustomerId").Value;
                            string orderDate = so.Element("OrderDate").Value;

                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@SalesOrderNumber", salesOrderNumber);
                            command.Parameters.AddWithValue("@CustomerId", customerId);
                            command.Parameters.AddWithValue("@OrderDate", orderDate);
                            int rowsAffected = command.ExecuteNonQuery();
                            Console.WriteLine($"{salesOrderNumber}\t{customerId} inserted");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        string disableIdentityInsert = "SET IDENTITY_INSERT [Sales].[SalesOrder] OFF";

                        using (SqlCommand command = new SqlCommand(disableIdentityInsert, connection))
                        {
                            //--< Turn Identity Insert Off so that don't need to input PKs >--//
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                }
            }
        }
        static void LoadParts()
        {
            string enableIdentityInsert = "SET IDENTITY_INSERT [Sales].[Part] ON";

            var insertSalesOrder = "INSERT INTO Sales.Part(PartId, Name, Price, ReceivedValue, ShippedValue, " +
                " SpoiledUnits, ReceivedUnits, ShippedUnits, SpoiledValue) " +
                "VALUES (@PartId, @Name, @Price, @ReceivedValue, @ShippedValue, " +
                "@SpoiledUnits, @ReceivedUnits, @ShippedUnits, @SpoiledValue)";
            XDocument xmlDoc = XDocument.Load(xmlData);
            var parts = xmlDoc.Descendants("Part");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open the database connection
                    using (SqlCommand command = new SqlCommand(enableIdentityInsert, connection))
                    {
                        //--< Turn Identity Insert On so that we can input PKs >--//
                        command.ExecuteNonQuery();

                        //--< In with the new data >--//
                        command.CommandText = insertSalesOrder;
                        // Add parameters to avoid SQL injection
                        foreach (var part in parts)
                        {
                            string partId = part.Element("PartId").Value;
                            string name = part.Element("Name").Value;
                            string price = part.Element("Price").Value;
                            string receivedValue = part.Element("ReceivedValue").Value;
                            string shippedValue = part.Element("ShippedValue").Value;
                            string spoiledValue = part.Element("SpoiledValue").Value;
                            string shippedUnits = part.Element("ShippedUnits").Value;
                            string spoiledUnits = part.Element("SpoiledUnits").Value;
                            string receivedUnits = part.Element("ReceivedUnits").Value;

                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@PartId", partId);
                            command.Parameters.AddWithValue("@Name", name);
                            command.Parameters.AddWithValue("@Price", price);
                            command.Parameters.AddWithValue("@ReceivedValue", receivedValue);
                            command.Parameters.AddWithValue("@ShippedValue", shippedValue);
                            command.Parameters.AddWithValue("@SpoiledValue", spoiledValue);
                            command.Parameters.AddWithValue("@ShippedUnits", shippedUnits);
                            command.Parameters.AddWithValue("@SpoiledUnits", spoiledUnits);
                            command.Parameters.AddWithValue("@ReceivedUnits", receivedUnits);
                            int rowsAffected = command.ExecuteNonQuery();
                            Console.WriteLine($"{partId}\t{name} inserted");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        string disableIdentityInsert = "SET IDENTITY_INSERT [Sales].[Part] OFF";

                        using (SqlCommand command = new SqlCommand(disableIdentityInsert, connection))
                        {
                            //--< Turn Identity Insert Off so that don't need to input PKs >--//
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                }
            }
        }
        static void LoadSalesOrderParts()
        {
            string enableIdentityInsert = "SET IDENTITY_INSERT [Sales].[SalesOrderPart] ON";

            var insertSalesOrderPart = "INSERT INTO Sales.SalesOrderPart(SalesOrderNumber, PartId, Quantity,  " +
                " UnitsShipped, UnitPrice, UnitCost) " +
                "VALUES (@SalesOrderNumber, @PartId, @Quantity, " +
                "@UnitsShipped, @UnitPrice, @UnitCost)";
            XDocument xmlDoc = XDocument.Load(xmlData);
            var sop = xmlDoc.Descendants("SalesOrderPart");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open the database connection
                    using (SqlCommand command = new SqlCommand(enableIdentityInsert, connection))
                    {
                        //--< Turn Identity Insert On so that we can input PKs >--//
                        //command.ExecuteNonQuery();

                        //--< In with the new data >--//
                        command.CommandText = insertSalesOrderPart;
                        // Add parameters to avoid SQL injection
                        foreach (var part in sop)
                        {
                            string salesOrderNumber = part.Element("SalesOrderNumber").Value;
                            string partId = part.Element("PartId").Value;
                            string quantity = part.Element("Quantity").Value;
                            string unitsShipped = part.Element("UnitsShipped").Value;
                            string unitPrice = part.Element("UnitPrice").Value;
                            string unitCost = part.Element("UnitCost").Value;

                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@SalesOrderNumber", salesOrderNumber);
                            command.Parameters.AddWithValue("@PartId", partId);
                            command.Parameters.AddWithValue("@Quantity", quantity);
                            command.Parameters.AddWithValue("@UnitsShipped", unitsShipped);
                            command.Parameters.AddWithValue("@UnitPrice", unitPrice);
                            command.Parameters.AddWithValue("@UnitCost", unitCost);
                            int rowsAffected = command.ExecuteNonQuery();
                            Console.WriteLine($"{partId}\t{salesOrderNumber} inserted");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        string disableIdentityInsert = "SET IDENTITY_INSERT [Sales].[SalesOrderPart] OFF";

                        using (SqlCommand command = new SqlCommand(disableIdentityInsert, connection))
                        {
                            //--< Turn Identity Insert Off so that don't need to input PKs >--//
                            //command.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
            }
        }
        static void AdoNet()
        {
            string iAm = @"
                DELETE FROM Sales.Student;
                INSERT INTO Sales.Student (Name) Values (@Name)
            ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open the database connection
                    using (SqlCommand command = new SqlCommand(iAm, connection))
                    {
                        int rowsAffected = 0;

                        // Add parameters to avoid SQL injection
                        command.Parameters.AddWithValue("@Name", "Jonathan Fairhurst");

                        // Execute the query
                        rowsAffected = 0;
                        rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"{rowsAffected} row(s) affected.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally               {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
        static void CreateXml()
        {
            string filePath = xmlNewData;
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,          // Make the XML file human readable
                NewLineOnAttributes = false
            };
            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                // Start the document
                writer.WriteStartDocument();

                // Write the root element
                writer.WriteStartElement("Customers");

                foreach (var customer in customers)
                {
                    // Write individual cust elements
                    writer.WriteStartElement("Customer");
                    writer.WriteElementString("CustomerId", customer.CustomerId.ToString());
                    writer.WriteElementString("Name", $"{customer.FirstName} {customer.LastName}");
                    writer.WriteElementString("TotalSales", customer.TotalSales.ToString());
                    writer.WriteElementString("ItemSold", customer.ItemsSold.ToString());
                    writer.WriteElementString("Orders", customer.SalesOrders.Count().ToString());
                    writer.WriteStartElement("Items");
                    foreach (var ci in customer.CustomerItems)
                    {
                        writer.WriteStartElement("Part");
                        writer.WriteElementString("PartId", ci.PartId.ToString());
                        writer.WriteElementString("Part", ci.Part);
                        writer.WriteElementString("Amount", ci.Amount.ToString());
                        writer.WriteElementString("Quantity", ci.Quantity.ToString());
                        writer.WriteElementString("Shipped", ci.Shipped.ToString());
                        writer.WriteElementString("BackOrdered", ci.BackOrdered.ToString());
                        writer.WriteEndElement();      // End of Items
                    }
                    writer.WriteEndElement();       //--< Ends the "Items" element <<<
                    writer.WriteEndElement();       //--< Ends the "Customer" element <<<
                }

                // End the root element
                writer.WriteEndElement();       //--< Ends the "Customers" element <<<

                // End the document
                writer.WriteEndDocument();
            }

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
        static void GetXmlData()
        {
            XDocument xmlDoc = XDocument.Load(xmlData);
            customers = GetCustomers(xmlDoc);
            parts = GetParts(xmlDoc);
            salesOrders = GetSalesOrders(xmlDoc);
            salesOrderParts = GetSalesOrderParts(xmlDoc);
            SetNavigation();
        }

        static void GetJsonData()
        {
            string json = File.ReadAllText(jsonData);
            
            /* Install package System.Memory */
            var project = JsonSerializer.Deserialize<ProjectData>(json);

            customers = project.Customers;
            parts = project.Parts;
            salesOrders = project.SalesOrders;
            salesOrderParts = project.SalesOrderParts;

            SetNavigation();
        }
        /// <summary>
        /// Establishes navigation properties and relationships between customers, sales orders, parts, and sales order
        /// parts.
        /// </summary>
        private static void SetNavigation()
        {
            var salesOrdersLookup = salesOrders.ToLookup(so => so.CustomerId);
            var salesOrderPartsLookup = salesOrderParts.ToLookup(sop => sop.SalesOrderNumber);
            foreach (var customer in customers)
            {
                customer.SalesOrders = salesOrders.Where(so => so.CustomerId == customer.CustomerId).ToList();
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
            var project = new ProjectData
            {
                Customers = customers,
                Parts = parts,
                SalesOrders = salesOrders,
                SalesOrderParts = salesOrderParts
            };
        }
    }
}