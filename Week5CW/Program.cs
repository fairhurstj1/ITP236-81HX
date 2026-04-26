/// <summary>
/// Week 5 Classwork - LINQ Demonstrations
/// This program demonstrates LINQ query capabilities including:
/// - Aggregate functions (Sum, Max, Min, Average)
/// - Lazy loading and deferred execution
/// - Lambda expressions for filtering and projecting data
/// - Working with custom objects (Student class)
/// - Query chaining and ordering
/// </summary>

using Collection;

Console.WriteLine("LINQ");

/// <remarks>
/// Section 1: Aggregate Functions
/// Demonstrates basic LINQ aggregation operations on a numeric array.
/// These methods: Sum, Max, Min, Average perform immediate execution (eager evaluation).
/// </remarks>
int[] sevens = [
    42, 7, 14, 63, 21, 70, 49, 28, 35, 56
];

int sum = sevens.Sum();
int max = sevens.Max();
int min = sevens.Min();
double avg = sevens.Average();
Console.WriteLine($"Sum: {sum}, Max: {max}, Min: {min}, Average: {avg}");

/// <remarks>
/// Section 2: Lazy Loading / Deferred Execution
/// Demonstrates how LINQ queries are not executed until explicitly evaluated (Count, ToList, foreach, etc.).
/// The Where() method returns an IEnumerable that captures the query logic but doesn't execute it immediately.
/// When Count() is called, x has been changed to 70, so the filter now checks n > 70, resulting in 0 matches.
/// This is a common source of confusion and demonstrates the importance of understanding query evaluation timing.
/// </remarks>
int x = 0;
var array = sevens.Where(n => n > x);
x = 70;
var count = array.Count();
Console.WriteLine($"Count: {count}");

/// <remarks>
/// Section 3: Working with Complex Objects
/// Demonstrates LINQ with a collection of Student objects from the Collection project.
/// Uses lambda expressions (s => s.GPA) to extract and aggregate specific properties.
/// The Average() method uses these lambdas to calculate statistics on object properties.
/// </remarks>
var students = Student.Students;
float gpa = students.Average(s => s.GPA);
double idAverage = students.Average(s => s.StudentId);

Console.WriteLine($"Average GPA: {gpa}, Average Student ID: {idAverage}");

/// <remarks>
/// Section 4: Filtering Objects and Lazy Evaluation
/// Demonstrates Where() filtering on complex objects with a deferred execution example.
/// The deansList variable holds the query definition (IEnumerable), not the actual results.
/// Lazy loading enables efficient querying of large datasets by deferring computation until needed.
/// </remarks>
var deansList = students.Where(s => s.GPA >= 3.0);
count = deansList.Count();
Console.WriteLine($"Number of students on the Dean's List: {count}");

/// <remarks>
/// Section 5: Query Chaining and Immediate Execution
/// Demonstrates method chaining to compose complex queries in a fluent style.
/// - Where() filters by GPA threshold
/// - Select() projects to extract only the Name property (data shaping)
/// - OrderBy() sorts alphabetically by name
/// - ToArray() forces immediate execution and materializes results into an array
/// This pattern is often called the "fluent interface" and is a hallmark of LINQ design.
/// </remarks>
var results = students
    .Where(s => s.GPA >= 3.0)
    .Select(s => s.Name)
    .OrderBy(name => name)
    .ToArray();

Console.WriteLine("Students on the Dean's List:");
foreach (var name in results)
{
    Console.WriteLine(name);
}


Console.ReadKey();