using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace LINQ2
{
    internal class Program
    {
        static List<Student> students = Student.Students;
        static List<Course> courses = Course.Courses;
        static List<Roster> rosters = Roster.Rosters;

        static List<Customer> customers = Customer.Customers;
        static List<SalesOrder> salesOrders = SalesOrder.SalesOrders;

        static void Main(string[] args)
        {
            SchoolExamples();
            // var gpas = students.GroupBy(s => s.Major);
            // foreach (var gpa in gpas)
            // {
            //     Console.WriteLine($"Major: {gpa.Key}");
            //     foreach (var student in gpa)
            //     {
            //         Console.WriteLine($"\tStudent: {student.Name}, GPA: {student.GPA}");
            //     }
            // }
            SalesExamples();

            Console.ReadKey();
        }
        #region School Examples
        static void SchoolExamples()
        {
            var studentSchedule = StudentQueryExample(); DisplayStudent("Query", studentSchedule);
            studentSchedule = MethodExample(); DisplayStudent("Method", studentSchedule);
            studentSchedule = SelectManyExample(); DisplayStudent("Select Many", studentSchedule);
            studentSchedule = GroupByExample(); DisplayStudent("Group By", studentSchedule);
            var courseRoster = CourseQueryExample(); DisplayCourse("Course Query", courseRoster);
        }

        static dynamic StudentQueryExample()
        {
            //--<QUERY METHOD>--//
            /*
            The join method has this signature:
            Join(outer,            "Students"
                 inner,            "Rosters"
                 outerKeySelector, "stu.StudentId"
                 innerKeySelector, "ros.StudentId"
                 resultSelector,   "result"
                 )
                 */
            var studentSchedule = 
                (from stu in students
                 join ros in rosters on stu.StudentId equals ros.StudentId
                 join course in courses on ros.CourseId equals course.CourseId
                 //note that the StudentId alone makes the Key unique. We add Name as an added property for easier display, but it's not necessary for the grouping to work.
                 group course by new { ros.StudentId, stu.Name } //StudentId and Name are the key for grouping. Each unique combination of StudentId and Name will create a new group.
                    into courseGroup            // Creates a group
                select new                      // Anonymous class
                {
                    StudentId = courseGroup.Key.StudentId,      // Note that we don't have to name the key properties "StudentId" and "Name". We can name them anything we want. The important part is that they are unique and can be used to identify the group.  
                    Student = courseGroup.Key.Name,
                    Count = courseGroup.Count(),                // Do "collection" things like Count, Average, Min, Max, etc. on the group
                    Courses = courseGroup.ToList()              // Create a collection of Courses from the group
                }
                );
            return studentSchedule;
        }

        static dynamic MethodExample()
        {
            //Same result using method style
            var studentSchedule = students
                .Join(rosters,
                    stu => stu.StudentId,
                    ros => ros.StudentId,
                    (stu, ros) => new { stu, ros })
                .Join(courses,
                    temp => temp.ros.CourseId,
                    course => course.CourseId,
                    (temp, course) => new 
                    {
                        temp.stu.StudentId,
                        temp.stu.Name,
                        Course = course
                        })
                .GroupBy( x => new { x.StudentId, x.Name })
                .Select(g => new
                {
                    StudentId = g.Key.StudentId,
                    Student = g.Key.Name,
                    Count = g.Count(),
                    Courses = g.Select(x => x.Course).ToList()
                });
            return studentSchedule;
        }
        static dynamic SelectManyExample()
        {
            //same result using SelectMany
            var studentSchedule = students
                .SelectMany(stu => rosters.Where(r => r.StudentId == stu.StudentId),
                        (stu, ros) => new { stu, ros })
                .SelectMany(x => courses.Where(c => c.CourseId == x.ros.CourseId),
                        (x, course) => new
                        {
                            x.stu.StudentId,
                            x.stu.Name,
                            Course = course
                        })
                .GroupBy(x => new { x.StudentId, x.Name })
                .Select(g => new
                {
                    StudentId = g.Key.StudentId,
                    Student = g.Key.Name,
                    Count = g.Count(),
                    Courses = g.Select(x => x.Course).ToList()
                });
            return studentSchedule;
        }
        static dynamic GroupByExample()
        {
            var groups = students.GroupBy(stu => stu.Major);
            /*
            1. After the joins, each row looks like:
            {
                StudentId,
                StudentName,
                Course
            }
            2. GroupBy groups those rows by:
                new {x.StudentId, x.StudentName}
                So each group represents one student.
            3. Inside each group, we now have:
                - All the courses that student is taking
                - The ability to call .Count(), .ToList(), .Select(), etc.
            4. The final projection builds a clean object:
                - StudentId,
                - Student Name,
                - Number of courses,
                - List of course objects


             */

             var studentSchedule = students
                    .Join(rosters,
                            stu=>stu.StudentId,
                            ros=>ros.StudentId,
                            (stu, ros) => new { stu, ros })
                    .Join(courses,
                            temp => temp.ros.CourseId,
                            course => course.CourseId,
                            (temp, course) => new
                            {
                                StudentId = temp.stu.StudentId,
                                StudentName = temp.stu.Name,
                                Course = course
                            })
                    .GroupBy(x => new { x.StudentId, x.StudentName })
                    .Select(g => new
                    {
                        StudentId = g.Key.StudentId,
                        Student = g.Key.StudentName,
                        Count = g.Count(),
                        Courses = g.Select(x => x.Course).ToList()
                    });
            return studentSchedule;
        }

        static dynamic CourseQueryExample()
        {
            var courseRoster = (
                from stu in students
                join ros in rosters on stu.StudentId equals ros.StudentId
                join course in courses on ros.CourseId equals course.CourseId
                group stu by new { ros.CourseId, course.Name } 
                    into stuGroup
                select new //CourseRoster
                {
                    CourseId = stuGroup.Key.CourseId,
                    Course = stuGroup.Key.Name,
                    Students = stuGroup.ToList(),
                    RosterCount = stuGroup.Count()
                }
            );
            //var classRoster = courses.GroupBy(
            //    course => course.CourseId,
            //    course => rosters.Where(r => r.CourseId == course.CourseId)
            //          .Select(r => students.FirstOrDefault(s => s.StudentId == r.StudentId)),
            //          (key, groupedStudents) => new
            //          {
            //              CourseId = key,
            //              Course = courses.First(c => c.CourseId == key).Name,
            //              Students = groupedStudents.SelectMany(st => st).ToList()
            //          }
            //        );
            return courseRoster;
        }

        #endregion

        #region School Displays

        static void DisplayStudent(string example, dynamic studentSchedule)
        {
            Console.WriteLine($"--- {example} ---");
            //list each student and the student's courses
            foreach (var ss in studentSchedule)
            {
                Console.WriteLine($"{ss.StudentId}\t{ss.Student}");
                foreach (var cc in ss.Courses)
                {
                    Console.WriteLine($"\t\t{cc.Name}");
                }
            }
        }

        static void DisplayCourse(string example, dynamic courseRoster)
        {
            Console.WriteLine($"\n--- {example} Example---");
            //list each course and the students in the course
            foreach (var cc in courseRoster)
            {
                Console.WriteLine($"{cc.CourseId}\t{cc.RosterCount}\t{cc.Course}");
                foreach (var stu in cc.Students)
                {
                    Console.WriteLine($"\t\t{stu.Name}");
                }
            }
        }

        #endregion

        #region Sales Examples

        static void SalesExamples()
        {
            var customerOrders = SalesQueryExample(); DisplayCustomer("Query", customerOrders);
            customerOrders = AnotherSalesQueryExample(); DisplayCustomer("Another Query", customerOrders);
            var regions = GroupByRegionExample(); DisplayRegion("Group By Region", regions);
            var selectedOrders = SalesContainsExample(); DisplayCustomer("Contains Example", selectedOrders);
        }

        static dynamic SalesQueryExample()
        {
            var customerOrders = from customer in customers
                                 join salesOrder in salesOrders
                                 on customer.CustomerId equals salesOrder.CustomerId
                                 select new
                                 {
                                     Customer = customer.Name,
                                     customer.CustomerId,
                                     salesOrder.OrderDate,
                                     salesOrder.OrderTotal
                                 };
            return customerOrders;
        }

        static dynamic AnotherSalesQueryExample()
        {
            var orders = from customer in customers
                                join salesOrder in salesOrders
                                    on customer.CustomerId equals salesOrder.CustomerId 
                                    into ordersGroup
                                select new
                                {
                                    Customer = customer.Name,
                                    customer.CustomerId,
                                    Count = ordersGroup.Count(),
                                    OrderTotal = ordersGroup.Sum(grp => grp.OrderTotal),
                                    SalesOrders = ordersGroup.ToList()
                                };
            return orders;
        }

        static dynamic GroupByRegionExample()
        {
            var regional = from c in customers
                          join so in salesOrders
                          on c.CustomerId equals so.CustomerId
                          into custOrders
                          group new {c, Orders = custOrders}
                            by c.Region into regionGroup
                          select new
                          {
                              Region = regionGroup.Key,
                              Customers = regionGroup
                          };
            return regional;
        }

        static dynamic SalesContainsExample()
        {
            int[] customerIds = { 1, 3 };
            var selectedOrders = (from order in salesOrders
                                 where customerIds
                                    .Contains(order.CustomerId)
                                 select order).OrderBy(o => o.CustomerId);
            return selectedOrders;
        }

        #endregion

        #region Sales Displays

        static void DisplayCustomer(string example, dynamic customerOrders)
        {
            Console.WriteLine($"--- {example} ---");
            foreach (var order in customerOrders)
            {
                var type = order.GetType();
                var customerId = type.GetProperty("CustomerId")?.GetValue(order);
                var customer = type.GetProperty("Customer")?.GetValue(order)?.ToString() ?? "(n/a)";
                var orderDateObj = type.GetProperty("OrderDate")?.GetValue(order);
                var countObj = type.GetProperty("Count")?.GetValue(order);
                var orderTotalObj = type.GetProperty("OrderTotal")?.GetValue(order);

                var dateOrCount = orderDateObj is DateTime date
                    ? date.ToString("yyyyMMdd")
                    : (countObj is not null ? $"Count: {countObj}" : "");

                var totalText = orderTotalObj is IFormattable formattable
                    ? formattable.ToString("C", null)
                    : (orderTotalObj?.ToString() ?? "");

                Console.WriteLine($"{customerId}\t{customer}\t{dateOrCount}\t{totalText}");
            }
        }

        static void DisplayRegion(string example, dynamic regions)
        {
            Console.WriteLine($"--- {example} ---");
            foreach (var region in regions)
            {
                Console.WriteLine($"{region.Region}");
                foreach (var cust in region.Customers)
                {
                    Console.WriteLine($"\t{cust.c.Name}");
                    foreach (var order in cust.Orders)
                    {
                        Console.WriteLine($"\t\t{order.OrderId}\t{order.OrderTotal}");
                    }
                }
            }
        }

        #endregion
        


    }

}