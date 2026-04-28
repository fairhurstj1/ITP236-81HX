//#define Part2 
//#define Part3
//#define Part4

/*
    Install the following libraries. Use Version 8.0.0 (not latest):
• 	  Microsoft.Extensions.Configuration
• 	  Microsoft.Extensions.Configuration.Json
•     Microsoft.Extensions.Configuration.FileExtensions
 
    In appsettings.json, modify Properties:
•     Set - Copy to Output Directory → Copy if newer
*/

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.EntityFrameworkCore;
using SchoolModel;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = config.GetConnectionString("SchoolDb");

var options = new DbContextOptionsBuilder<SchoolContext>()
    .UseSqlServer(connectionString)
    .Options;

using var db = new SchoolContext(options);
var factory = new SchoolContextFactory();
using var context = factory.CreateDbContext(args);

// Only seed if empty
if (!context.Students.Any())
{
    var ada = new Student { FirstName = "Ada", LastName = "Lovelace", Major = "Math" };
    var grace = new Student { FirstName = "Grace", LastName = "Hopper", Major = "CS" };

    var cs101 = new Course { Tag = "CS101", Title = "Intro to Programming", Credits = 3 };
    var math200 = new Course { Tag = "MATH200", Title = "Discrete Math", Credits = 4 };

    context.Students.AddRange(ada, grace);
    context.Courses.AddRange(cs101, math200);

    context.SaveChanges();

    context.Enrollments.AddRange(
#if Part3 
        new Enrollment { StudentId = ada.StudentId, CourseId = cs101.CourseId, Grade = "A" },
        new Enrollment { StudentId = grace.StudentId, CourseId = math200.CourseId, Grade = "B" }
#else
        new Enrollment { StudentId = ada.StudentId, CourseId = cs101.CourseId },
        new Enrollment { StudentId = grace.StudentId, CourseId = math200.CourseId }
#endif 
    );

    context.SaveChanges();
}

Console.WriteLine("Database seeded.");


// test query
Console.WriteLine($"Students: {db.Students.Count()}");

Console.WriteLine("EF Core Sandbox");
Console.WriteLine("----------------");

while (true)
{
    Console.Write("Command> ");
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
        break;

    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    try
    {
        switch (input.ToLower())
        {
            case "students":
                var students = context.Students
                    .AsNoTracking()                 //--< We will not be doing Updates <<<
                    .Include(s => s.Enrollments)
                        .ThenInclude(e => e.Course)
                    .ToList();
                foreach (var s in students)
                {
                    Console.WriteLine($"{s.StudentId}: {s.FirstName} {s.LastName} ({s.Major})");
                    Console.WriteLine("\tCourses Enrolled In:");
                    foreach (var e in s.Enrollments)
                    {
                        Console.WriteLine($"\t{e.Course.Tag}: {e.Course.Credits} Credits\t {e.Course.Title}");
                    }
                }
                break;

            case "courses":
                var courses = context.Courses
                    .Include(c => c.Enrollments)
                        .ThenInclude(e => e.Student)
                    .ToList();
                foreach (var c in courses)
                {
                    Console.WriteLine($"{c.CourseId}: {c.Tag} - {c.Title}\tCredits: {c.Credits}");
                    Console.WriteLine("\tStudents Enrolled");
                    foreach (var e in c.Enrollments)
                    {
#if Part2 
                        Console.WriteLine($"\t{e.Student.FullName}\tMajor: {e.Student.Major}\tPhone: {e.Student.PhoneNumber}");
#else
                        Console.WriteLine($"\t{e.Student.FullName}\tMajor: {e.Student.Major}\tPhone:");
#endif
                    }
                }
                break;

            case "enrollments":
                var enrollments = context.Enrollments
                    .Include(e => e.Course)
                    .Include(e => e.Student)
                    .ToList();
                foreach (var e in enrollments)
                    Console.WriteLine($"{e.Course.Tag} {e.Course.Title} : {e.Student.FullName}");
                break;

            case "inserts":
                var student = new Student() { FirstName = "James", LastName = "River", Major = "Programming" };
                db.Students.Add(student);

                var course = new Course() { Tag = "ITP-245", Title = "User Interface", Credits = 4 };
                db.Courses.Add(course);

                db.SaveChanges();       //--< Insert student and course <<<

                var enrollment = new Enrollment() { CourseId = course.CourseId, StudentId = student.StudentId };
                db.Enrollments.Add(enrollment);
                db.SaveChanges();       //--< Insert enrollment <<<
                break;

            case "updates": 
                student = db.Students
                    //.AsNoTracking()                       //--< Turn this off because we will update <<<
                    .OrderBy(s => s.StudentId).Last();      //--< Update the last Student inserted <<<
                if (student != null)
                    student.Major = "Finance";
                course = db.Courses.OrderBy(c => c.CourseId).Last();
                if (course != null)
                    course.Credits = 5;
                db.SaveChanges();
                break;

            case "deletes":
                student = db.Students
                    .OrderBy(s => s.StudentId).Last();     //--< Delete the last Student inserted <<<
                if (student != null)
                    db.Students.Remove(student);

                course = db.Courses.OrderBy(c => c.CourseId).Last();
                if (course != null)
                    db.Courses.Remove(course);

                enrollment = db.Enrollments.OrderBy(e => e.EnrollmentId).Last();
                //--< With Cascade Delete, we don't need this next statement >--//
                //db.Enrollments.Remove(enrollment);

                db.SaveChanges();
                break;

            case "Clubs":
#if Part4
                var clubs = context.Clubs
                    .Include(c => c.Members)
                    .ToList();
                foreach(var c in clubs)
                {
                    Console.WriteLine($"{c.Name} Members");
                    foreach (var m in c.Members)
                    {
                        Console.WriteLine($"\t{m.FullName}");
                    }
                }
#else
                Console.WriteLine("Clubs has not been implemented");
#endif
                break;
            default:
                Console.WriteLine("Unknown command.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

//Console.ReadKey();