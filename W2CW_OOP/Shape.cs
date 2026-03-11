namespace Shapes
{
    internal abstract class Shape
    {
        public string Name { get; set; }
        //public Shape() {Name = "Unknown Shape";} - bad idea: changing Name in multiple places can cause confusion in debugging
        public Shape(string name)
        {
            Name = name;
        }
        public Shape() : this("Unknown Shape") { } // this sets Name in the constructor to "Unknown Shape". It references the same variable assignment, keeping the data flow easy to navigate.
        public abstract double Area();
        public abstract double Perimeter();
        public virtual string Description()
        {
            return $"The {Name} has an area of {Area():F2} and a perimeter of {Perimeter():F2}.";
        }
    }
}