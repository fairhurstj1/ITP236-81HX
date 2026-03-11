namespace Shapes
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Shapes:\n");
            // Console.WriteLine("Circles:");
            // Circle circle1 = new Circle("Circle", 5.0);
            // Console.WriteLine(circle1.Description());

            // Circle circle2 = new Circle(3.0); // uses default name "Circle"
            // Console.WriteLine(circle2.Description());

            // Circle circle3 = new Circle(); // uses default name "Circle" and default radius 1.0
            // Console.WriteLine(circle3.Description());

            // Console.WriteLine("\nRectangles:");
            // Rectangle rectangle1 = new Rectangle("Rectangle", 4.0, 6.0);
            // Console.WriteLine(rectangle1.Description());

            // Rectangle rectangle2 = new Rectangle(5.0, 4.0); // uses default name "Rectangle"
            // Console.WriteLine(rectangle2.Description());

            // Rectangle rectangle3 = new Rectangle(); // uses default name "Rectangle" and default width and height of 1.0
            // Console.WriteLine(rectangle3.Description());

            // Console.WriteLine("\nSquares:");
            // Square square1 = new Square("Square", 4.0);
            // Console.WriteLine(square1.Description());

            // Square square2 = new Square(5.0); // uses default name "Square"
            // Console.WriteLine(square2.Description());

            // Square square3 = new Square(); // uses default name "Square" and default side length of 1.0
            // Console.WriteLine(square3.Description());

            List<Shape> shapes = new List<Shape>
            {
                new Circle("My Circle", 5.0),
                new Rectangle("My Rectangle", 4.0, 6.0),
                new Square("My Square", 4.0),
                new Circle(3.0), // uses default name "Circle"
                new Rectangle(5.0, 4.0), // uses default name "Rectangle"
                new Square(5.0), // uses default name "Square"
                new Circle(), // uses default name "Circle" and default radius 1.0
                new Rectangle(), // uses default name "Rectangle" and default width and height of 1
                new Square() // uses default name "Square" and default side length of 1.0
            };

            foreach (var shape in shapes)
            {
                Console.WriteLine(shape.Description());
            }
        }
    }
}