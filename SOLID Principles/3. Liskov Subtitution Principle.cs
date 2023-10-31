using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID_Principles
{
    // You should be able to subtitute a base type for a sub type 
    public class Liskov_Subtitution_Principle
    {
        static public int Area(Rectangle r) => r.Width * r.Height;

        public static void Main(string[] args)
        {
            Rectangle r = new Rectangle(2, 3);
            Console.WriteLine(Area(r));

            Rectangle sq = new Square(); // so now instand of Square we can use an instance of Rectangle  ( Inheritence of a square is a rectangle )
            sq.Width = 4;
            sq.Height = 3;
            Console.WriteLine(Area(sq));
        }
    }

    public class Rectangle
    {
        public virtual int Width {  get; set; } // virtual prop to override
        public virtual int Height { get; set; }

        public Rectangle() { }
        public Rectangle(int width, int height) 
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }

    public class Square : Rectangle
    {
        public override int Width { 
            set { base.Width = base.Height = value; } // override prop 
        } 

        public override int Height
        {
            set { base.Height = base.Width = value;}
        }

    }
}
