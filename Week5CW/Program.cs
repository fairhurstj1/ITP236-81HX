using Collection;

Console.WriteLine("LINQ");
int[] sevens = [
    42, 7, 14, 63, 21, 70, 49, 28, 35, 56
];

// Using LINQ
int sum = sevens.Sum();
int max = sevens.Max();
int min = sevens.Min();
double avg = sevens.Average();
Console.WriteLine($"Sum: {sum}, Max: {max}, Min: {min}, Average: {avg}");

//Lazy loading (explanation: the query is not executed until we actually iterate over it or call a method that forces execution, like Count())
int x = 0;
var array = sevens.Where(n => n > x); // Lambda expression to filter numbers greater than x
x = 70;
var count = array.Count(); //Where is not executed until we call Count(), and at that time, x is 70, so it counts how many numbers are greater than 70, which is 0.
//Why do I get Count = 0? Because the query is evaluated at the time of execution, and at that time, x is 70, so it counts how many numbers are greater than 70, which is 0.
Console.WriteLine($"Count: {count}");

//Using Student class from Collection project
var students = Student.Students;
float gpa = students.Average(s => s.GPA); // Lambda expression to calculate average GPA (s => s.GPA means for each student s, take their GPA)
double idAverage = students.Average(s => s.StudentId);

Console.WriteLine($"Average GPA: {gpa}, Average Student ID: {idAverage}");

//another example of lazy loading with students
var deansList = students.Where(s => s.GPA >= 3.0); // Deans list is a DELEGATE that filters students with a GPA of 3.0 or higher, but it is not executed until we call Count() or iterate over it.
count = deansList.Count(); // This will count how many students have a GPA of 3.0 or higher
//lazy loading allows us to define complex queries without immediately executing them, which can improve performance and reduce memory usage when working with large datasets.
Console.WriteLine($"Number of students on the Dean's List: {count}");

var results = students
    .Where(s => s.GPA >= 3.0) // Filter students with a GPA of 3.0 or higher
    .Select(s => s.Name) // Select only the names of the students
    .OrderBy(name => name) // Order the names alphabetically
    .ToArray(); // Convert the result to an array. The query is executed when we call ToArray() (No lazy loading), which retrieves the names of the students on the Dean's List, orders them alphabetically, and stores them in an array.

Console.WriteLine("Students on the Dean's List:");
foreach (var name in results)
{
    Console.WriteLine(name);
}


Console.ReadKey();