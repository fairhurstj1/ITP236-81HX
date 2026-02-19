using System;
using System.Collections.Generic;
namespace EID
{
    public class Program
    {
        static void Main(string[] args)
        {
            string userName = "";
            string properName = "";
            int faveNumber = 0;
            string ordinal = "";
            //ProperName extension method execution
            Console.WriteLine("Enter your name, and feel free to capitalize it however you like:");
            userName = Console.ReadLine() ?? string.Empty;
            properName = Extension.ProperName(userName);
            Console.WriteLine($"Original: {userName}");
            Console.WriteLine($"Proper: {properName}");

            //Ordinal extension method execution
            Console.WriteLine("\nEnter your favorite number:");
            //Error handling
            try
            {
                faveNumber = int.Parse(Console.ReadLine() ?? "0");
            }//try
            catch (FormatException)
            {
                Console.WriteLine("That's not a valid number. Defaulting to 0.");
                faveNumber = 0;
            }//catch Format
            catch (OverflowException)
            {
                Console.WriteLine("That's not a valid number. Defaulting to 0.");
                faveNumber = 0;
            }//catch Overflow
            catch (Exception e)
            {
                Console.WriteLine($"An unexpected error occurred: {e.Message}. Defaulting to 0.");
                faveNumber = 0;
            }//catch Exception

            //Handle negative numbers
            if(faveNumber < 0)
            {
                Console.WriteLine("Negative numbers do not have ordinals. Defaulting to 0.");
                faveNumber = 0;
            }//if

            ordinal = Extension.Ordinal(faveNumber);
            Console.WriteLine($"Number: {faveNumber}");
            Console.WriteLine($"Ordinal: {ordinal}");

            //Interface execution
            Teacher teacher = new Teacher("John", "Doe", 35, 1989, "Mathematics");
            Student student = new Student("Jane", "Smith", 20, 2004, "Sophomore");
            Administrator admin = new Administrator("Alice", "Johnson", 45, 1979, "Admissions");

            Display(teacher);
            Console.WriteLine();
            Display(student);
            Console.WriteLine();
            Display(admin);
            Console.WriteLine();
        }//Main

        //Display method for IPerson objects
        public static void Display(IPerson person)
        {
            //Handle nulls
            if (person == null)
            {
                Console.WriteLine("No person information to display.");
                return;
            }//if
            //Use pattern matching to determine the type of IPerson and display relevant info
            else if (person is Teacher teacher)
            {
                Console.WriteLine("{0,-25}{1,-25}{2,-10}{3,-15}{4}", "First Name", "Last Name", "Age", "Birth Date", "Subject");
                Console.WriteLine("{0,-25}{1,-25}{2,-10}{3,-15}{4}", person.FirstName, person.LastName, person.Age, person.BirthDate, teacher.Subject);
            }//else if teacher
            else if (person is Student student)
            {
                Console.WriteLine("{0,-25}{1,-25}{2,-10}{3,-15}{4}", "First Name", "Last Name", "Age", "Birth Date", "Grade Level");
                Console.WriteLine("{0,-25}{1,-25}{2,-10}{3,-15}{4}", person.FirstName, person.LastName, person.Age, person.BirthDate, student.GradeLevel);
            }//else if student
            else if (person is Administrator administrator)
            {
                Console.WriteLine("{0,-25}{1,-25}{2,-10}{3,-15}{4}", "First Name", "Last Name", "Age", "Birth Date", "Department");
                Console.WriteLine("{0,-25}{1,-25}{2,-10}{3,-15}{4}", person.FirstName, person.LastName, person.Age, person.BirthDate, administrator.Department);
            }//else if administrator
            //catch-all else
            else
            {
                Console.WriteLine("Person Type: Unknown");
            }//else
        }

    }//Program

    
    //Teacher class implementing IPerson
    public class Teacher : IPerson
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int Age { get; set; }
        public int BirthDate { get; set; }
        public string Subject { get; set; } = "";

        //default constructor
        public Teacher() { }
        //parameterized constructor
        public Teacher(string firstName, string lastName, int age, int birthDate, string subject)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            BirthDate = birthDate;
            Subject = subject;
        }
    }

    //Student class implementing IPerson
    public class Student : IPerson
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int Age { get; set; }
        public int BirthDate { get; set; }
        public string GradeLevel { get; set; } = "";

        //default constructor
        public Student() { }
        //parameterized constructor
        public Student(string firstName, string lastName, int age, int birthDate, string gradeLevel)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            BirthDate = birthDate;
            GradeLevel = gradeLevel;
        }
    }

    //Administrator class implementing IPerson
    public class Administrator : IPerson
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int Age { get; set; }
        public int BirthDate { get; set; }
        public string Department { get; set; } = "";

        //default constructor
        public Administrator() { }
        //parameterized constructor
        public Administrator(string firstName, string lastName, int age, int birthDate, string department)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            BirthDate = birthDate;
            Department = department;
        }
    }
}//EID