namespace Shapes
{
    internal class Rectangle : Shape
    {
        public double Width { get; set; } // this property allows the user to set the width of the rectangle, which is necessary for calculating the area and perimeter.
        public double Height { get; set; } // this property allows the user to set the height of the rectangle, which is necessary for calculating the area and perimeter.
        public Rectangle(string name, double width, double height) : base(name) // this constructor allows the user to set both the name, width, and height of the rectangle, while also calling the base constructor to set the name property.
        {
            Width = width;
            Height = height;
        }
        public Rectangle(double width, double height) : this("Rectangle", width, height) { } // default name of "Rectangle"
        public Rectangle() : this(1.0, 1.0) { } // default width and height of 1.0
        public override double Area() // this method overrides the abstract Area method in the Shape class, providing a specific implementation for calculating the area of a rectangle using the formula A = w * h.
        {
            return Width * Height;
        }
        public override double Perimeter() // this method overrides the abstract Perimeter method in the Shape class, providing a specific implementation for calculating the perimeter of a rectangle using the formula P = 2(w + h).
        {
            return 2 * (Width + Height);
        }
        public override string Description() // this method overrides the virtual Description method in the Shape class, providing a specific implementation for describing the rectangle, including its name, width, height, area, and perimeter.
        {
            // if (IsSquare())
            // {
            //     return $"The {Name} is a square with a width and height of {Width:F2}, an area of {Area():F2}, and a perimeter of {Perimeter():F2}.";
            // }
            // else
            // {
            //     return $"The {Name} has a width of {Width:F2}, a height of {Height:F2}, an area of {Area():F2}, and a perimeter of {Perimeter():F2}.";
            // }
            return $"This is a rectangle with name {Name}, has a width of {Width:F2}, a height of {Height:F2}, an area of {Area():F2}, and a perimeter of {Perimeter():F2}.";
        }
        // public Boolean IsSquare() // this method checks if the rectangle is a square by comparing the width and height properties. If they are equal, it returns true; otherwise, it returns false.
        // {
        //     if (Width == Height)
        //     {
        //         return true;
        //     }
        //     else
        //     {
        //         return false;
        //     }
        // }

        // I've elected to use a subclass for Square instead of a method to check if a rectangle is a square. 
        // This is because a square is a specific type of rectangle with equal sides, and it allows for better organization 
        // and potential future expansion (e.g., adding specific properties or methods for squares). The IsSquare method 
        // could be useful in some contexts, but it may not be necessary if we have a dedicated Square class that inherits from Rectangle.
    }
}