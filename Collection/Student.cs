using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Collection
{
    #region Student
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public float GPA { get; set; }

        /*
         Dynamic Sizing:
            A List can grow or shrink in size as elements are added or removed.
            It automatically adjusts its capacity to accommodate new elements.
        Type-Safe:
            As a generic collection (List<T>), a List enforces type safety, ensuring that only elements of the specified type can be added.
        Indexed Access:
            Provides zero-based index access to elements, allowing for quick retrieval and modification of elements by their position in the list.
         */
        public static List<Student> Students
        {
            get
            {
                var list = new List<Student>();
                foreach (var student in students)
                {
                    list.Add(student);
                }
                return list;
            }
        }

        /*
         Key-Value Pairs:
        Each element in a Dictionary is stored as a key-value pair.
        The key must be unique, while the value can be duplicated.
        Fast Lookups:
            Provides very fast access to values by their keys using a hash table.
            Lookup, addition, and deletion operations are generally performed in constant time, O(1).
        Type-Safe:
            Being a generic collection (Dictionary<TKey, TValue>), it enforces type safety, ensuring that only elements of the specified types can be added.
        Dynamic Size:
            The size of a Dictionary can grow dynamically as elements are added or removed.
        Indexers:
            Allows access to elements using an indexer, providing an intuitive syntax for retrieving and setting values.
         */
        public static Dictionary<int, Student> StudentDictionary
        {
            get
            {
                var dictionary = new Dictionary<int, Student>();
                foreach (var student in students)
                {
                    dictionary.Add(student.StudentId, student);
                }
                return dictionary;
            }
        }
        /* 
           LIFO Order:
           Elements are added and removed from the top of the stack.
           The last element added is the first one to be removed.
        */

        public static Stack<Student> StudentStack
        {
            get
            {
                var stack = new Stack<Student>();
                foreach (var student in students)
                {
                    stack.Push(student);
                }
                return stack;
            }
        }

        /*
         FIFO Order:
         Elements are added at the back (enqueue) and removed from the front (dequeue).
         The first element added is the first one to be removed.
         */

        public static Queue<Student> StudentQueue
        {
            get
            {
                var queue = new Queue<Student>();
                foreach (var student in students)
                {
                    queue.Enqueue(student);
                }
                return queue;
            }
        }
        //        Key Features of HashSet:
        //          Unique Elements: A HashSet only stores unique elements.If you try to add a duplicate element, it will be ignored.
        //          Fast Operations: Operations such as adding, removing, and checking for the existence of elements are very fast, generally performed in constant time.
        //          Unordered: The elements in a HashSet are not stored in any particular order.
        //          No Index: You cannot access elements in a HashSet by index.
        public static HashSet<Student> StudentHashSet
        {
            get
            {
                var hashSet = new HashSet<Student>();
                foreach (var student in students)
                {
                    hashSet.Add(student);
                }
                return hashSet;
            }
        }
        //public static SortedList<string, Student> StudentSortedList
        //{
        //    get
        //    {
        //        var sortedList = new SortedList<string, Student>();
        //        foreach (var student in students)
        //        {
        //            sortedList.Add(student.Name, student);
        //        }
        //        return sortedList;
        //    }
        //}

        //OrderedDictionary is a collection of key-value pairs, where the keys are ordered by the order they were added to the collection.
        public static OrderedDictionary StudentOrderedDictionary
        {
            get
            {
                var orderedDictionary = new OrderedDictionary();
                foreach (var student in students)
                {
                    orderedDictionary.Add(student.StudentId, student);
                }
                return orderedDictionary;
            }
        }


        private static Student[] students =
        {
            new Student(101, "Skip Wythe", 3.43f),
            new Student(108, "James River", 2.89f),
            new Student(107, "Maggie Walker", 3.49f),
            new Student(105, "Tre de Gar", 2.77f),
            new Student(109, "Chip N. Ham", 3.08f)
        };
        public Student(int studentId, string name, float gpa)
        {
            StudentId = studentId;
            Name = name;
            GPA = gpa;
        }
    }
    #endregion
}
