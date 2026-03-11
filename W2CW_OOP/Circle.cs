namespace Shapes
{
    internal class Circle : Shape
    {
        public double Radius { get; set; } // this property allows the user to set the radius of the circle, which is necessary for calculating the area and perimeter.
        public Circle(string name, double radius) : base(name) // this constructor allows the user to set both the name and radius of the circle, while also calling the base constructor to set the name property.
        {
            Radius = radius;
        }
        public Circle(double radius) : this("Circle", radius) { } // default name of "Circle"
        public Circle() : this(1.0) { } // default radius of 1.0
        public override double Area() // this method overrides the abstract Area method in the Shape class, providing a specific implementation for calculating the area of a circle using the formula A = πr^2.
        {
            return Math.PI * Math.Pow(Radius, 2);
        }
        public override double Perimeter() // this method overrides the abstract Perimeter method in the Shape class, providing a specific implementation for calculating the perimeter (circumference) of a circle using the formula C = 2πr.
        {
            return 2 * Math.PI * Radius;
        }
        public override string Description() // this method overrides the virtual Description method in the Shape class, providing a specific implementation for describing the circle, including its name, radius, area, and perimeter.
        {
            return $"This is a circle with name {Name}, has a radius of {Radius:F2}, an area of {Area():F2}, and a circumference of {Perimeter():F2}.";
        }
    }
}