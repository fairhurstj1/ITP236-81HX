// See https://aka.ms/new-console-template for more information
Console.WriteLine("LINQ");
int[] sevens = new int[] { 7, 14, 21, 28, 35, 42, 49, 56, 63, 70 };
// Use LINQ - Language Integrated Query
int sum = sevens.Sum();
int max = sevens.Max();
double avg = sevens.Average();
int min = sevens.Min();
int count = sevens.Count();
int first = sevens.First();
int last = sevens.Last();
int elementAt = sevens.ElementAt(3);
int singleOrDefault = sevens.SingleOrDefault(x => x == 35);
double standardDeviation = Math.Sqrt(sevens.Average(x => Math.Pow(x - avg, 2)));
Console.WriteLine($"The sum of the sevens is {sum}");
Console.WriteLine($"The max of the sevens is {max}");
Console.WriteLine($"The average of the sevens is {avg}");
Console.WriteLine($"The min of the sevens is {min}");
Console.WriteLine($"The count of the sevens is {count}");
Console.WriteLine($"The first element of the sevens is {first}");
Console.WriteLine($"The last element of the sevens is {last}");
Console.WriteLine($"The element at index 3 of the sevens is {elementAt}");
Console.WriteLine($"The single or default element of the sevens that is 35 is {singleOrDefault}");
Console.WriteLine($"The standard deviation of the sevens is {standardDeviation}");

//LINQ automatically attaches to C# projects allowing the user to use any LINQ properties automatically.
