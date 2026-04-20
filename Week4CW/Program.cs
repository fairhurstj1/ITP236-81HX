using System;
using System.Collections.Generic;
using Week4CW;


namespace Week4CW
{
    internal class Program
    {
        delegate int Numbers(int[] numbers);
        static void Main(string[] args)
        {
            #region Extension/Interface Method Tests
            Console.WriteLine("hello.".IsCapitalized());
            Console.WriteLine("Jonathan Fairhurst".Left(8));
            Console.WriteLine("Jonathan Fairhurst".Right(9));

            var consumer = new Consumer { CustomerId = 1, Name = "John Doe", TotalAmount = 1000.0 };
            var vendor = new Vendor { CustomerId = 2, Name = "Acme Corp", TotalAmount = 5000.0 };
            var businessAssociates = new List<IBusinessAssociate> { consumer, vendor };
            foreach (var associate in businessAssociates)
            {
                Console.WriteLine($"ID: {associate.Id},\t Name: {associate.Name},\t Total Amount: {associate.TotalAmount}");
            }
            #endregion
            #region Delegate Tests
            int[] sevens = [
                42, 7, 14, 63, 21, 70, 49, 28, 35, 56
            ];

            Numbers math = EidDelegate.SumArray;
            int result = math(sevens);
            Console.WriteLine("the sum is " + result);

            math = EidDelegate.Max;
            result = math(sevens);
            Console.WriteLine("the max is " + result);

            math = EidDelegate.Min;
            result = math(sevens);
            Console.WriteLine("the min is " + result);

            Console.ReadKey();
            #endregion
        }
    }
}