namespace Shapes
{
    internal class Square : Rectangle
    {
        
        public Square(string name, double side) : base(name, side, side) // this constructor allows the user to set both the name and side length of the square, while also calling the base constructor to set the name property and ensure that width and height are equal.
        {
        }
        public Square(double side) : this("Square", side) { } // default name of "Square"
        public Square() : this(1.0) { } // default side length of 1.0
        public override string Description() // this method overrides the virtual Description method in the Shape class, providing a specific implementation for describing the square, including its name, side length, area, and perimeter.
        {
            return $"This is a square with name {Name}, has a side length of {Width:F2}, an area of {Area():F2}, and a perimeter of {Perimeter():F2}.";
        }
    }
}