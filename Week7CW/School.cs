using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ2
{
    /*
           +----------------+        +----------------+        +----------------+
           |    Student     | 1 :  M |     Roster     | M  : 1 |     Course     |
           +----------------+--------+----------------+--------+----------------+
           | StudentId (PK) |        | RosterId (PK)  |        | CourseId (PK)  |
           | Name           |        | StudentId (FK) |        | CourseName     |
           | GPA (TBD)      |        | CourseId (FK)  |        | Credits        |
           | Major          |        | Grade          |        | Department     |
           +----------------+        +----------------+        +----------------+

            Student → Roster
                - A Student can appear in the Roster many times (one entry per course).
                - A Roster entry belongs to exactly one Student.
            Course → Roster
                - A Course can have many Roster entries (one per enrolled student).
                - A Roster entry belongs to exactly one Course.
            Roster as the Bridge Table
                - Roster is the junction table (many‑to‑many resolver).
                - It connects Students ↔ Courses.
                - It may also hold metadata like:
                    - DateEnrolled
                    - Grade
                    - Status (Active, Dropped, Completed)
    */
    /* According to CoPilot, the "=>" is called an expression‑bodied member operator.
         More formally, it’s the lambda operator, 
         but in this specific usage the C# language refers to the entire feature as an 
         expression‑bodied member (introduced in C# 6 and expanded in later versions).
       Think of the right side of "=>" as a delegate
    */

    /// <summary>
    /// Represents a student with an ID, name, and a collection of course enrollments.
    /// </summary>
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Major { get; set; }
        /// <summary>
        /// Gets the list of rosters for this Student
        /// </summary>
        public List<Roster> Rosters { get; private set; }
        /// <summary>
        /// Gets the number of enrollments in the Rosters collection.
        /// </summary>
        public int EnrollmentCount => Rosters.Count();
        public float GPA
        {
            get
            {
                Rosters = Roster.Rosters.Where(r => r.StudentId == this.StudentId).ToList();
                {
                    if (Rosters == null || !Rosters.Any())
                        return 0f;

                    var gradePoints = new Dictionary<string, float>
                        {
                            { "A", 4.0f },
                            { "B", 3.0f },
                            { "C", 2.0f },
                            { "D", 1.0f },
                            { "F", 0.0f }
                        };

                    var enrollments = Rosters
                        .Join(Course.Courses,
                            roster => roster.CourseId,
                            course => course.CourseId,
                            (roster, course) => new
                            {
                                GradePoint = gradePoints.ContainsKey(roster.Grade) ? gradePoints[roster.Grade] : 0f,
                                Credits = course.Credits
                            })
                        .ToList();

                    if (!enrollments.Any())
                        return 0f;

                    float totalPoints = enrollments.Sum(e => e.GradePoint * e.Credits);
                    int totalCredits = enrollments.Sum(e => e.Credits);

                    return totalCredits > 0 ? totalPoints / totalCredits : 0f;
                }
            }
        }//--< Rewrite to use CoPilot to calculate a Student's GPA <<<
        public Student()
        {
            Rosters = Roster.Rosters.Where(r => r.StudentId == this.StudentId).ToList();
        }
        public static List<Student> Students => new List<Student>
        {
            new Student { StudentId = 101, Name = "Alice", Major="Stem"},
            new Student { StudentId = 102, Name = "Becky", Major="Stem" },
            new Student { StudentId = 103, Name = "Charlie", Major = "History"}
        };
    }
    /// <summary>
    /// Represents a course with an identifier, name, and associated rosters.
    /// </summary>
    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public string Department { get; set; }
        /// <summary>
        /// Gets the list of rosters for this Course
        /// </summary>
        public List<Roster> Rosters { get; private set; }
        public Course()
        {
            Rosters = Roster.Rosters
                .Where(r => r.CourseId == this.CourseId).ToList();
        }
        public int EnrollmentCount => Rosters.Count();
        /// <summary>
        /// Gets a list of predefined courses including Mathematics, Science, and History.
        /// </summary>
        public static List<Course> Courses => new List<Course>
        {
            new Course { CourseId = 201, Name = "Mathematics", Credits = 3, Department = "STEM" },
            new Course { CourseId = 202, Name = "Science", Credits = 4, Department = "STEM"  },
            new Course { CourseId = 203, Name = "History", Credits = 3, Department = "History"  }
        };
    }
    /// <summary>
    /// Represents the enrollment of one student in one course.
    /// </summary>
    public class Roster
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string Grade { get; set; }
        public static List<Roster> Rosters => new List<Roster> {
            new Roster { StudentId = 101, CourseId = 201, Grade = "A" },
            new Roster { StudentId = 102, CourseId = 203, Grade = "C"  },
            new Roster { StudentId = 103, CourseId = 202, Grade = "B"  },
            new Roster { StudentId = 102, CourseId = 202, Grade = "B"  },
            new Roster { StudentId = 103, CourseId = 201, Grade = "A"  },
            new Roster { StudentId = 102, CourseId = 201, Grade = "D"  }
        };
    }
    /// <summary>
    /// Represents a course and its associated student roster.
    /// This is used in lieu of an anonymous class for teaching purposes
    /// </summary>
    public class CourseRoster
    {
        public int CourseId { get; set; }
        public string Course { get; set; }
        public int RosterCount { get; set; }
        public List<Student> Students { get; set; }
    }
}