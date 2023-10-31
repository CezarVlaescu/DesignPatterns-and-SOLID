using MoreLinq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamma_Categorization_Structural_Patterns
{
    // think of an adapter for electricity, we cannot modify our gadgets to support every possible interface
    // def -> a construct which adapts an existing interface X to confirm to the require intarface Y

    public class Point
    {
        public int X, Y;
        public Point(int x, int y) 
        {
            X = x;
            Y = y;
        }
    }

    public class Line
    {
        public Point Start, End;

        public Line(Point start, Point end)
        {
            Start = start;
            End = end;
        }
    }

    public class VectorObject : Collection<Line>
    {

    }

    public class VectorRectangle : VectorObject
    {
        public VectorRectangle(int x, int y, int width, int height) 
        {
            Add(new Line(new Point(x, y), new Point(x + width, y)));
            Add(new Line(new Point(x + width, y), new Point(x + width, y + height)));
            Add(new Line(new Point(x, y), new Point(x, y + height)));
            Add(new Line(new Point(x, y + height), new Point(x + width, y + height)));
        }
    }

    public class LineToPointAdapter : Collection<Point> // we read IEnumerable
    {
        static Dictionary<int, List<Point>> cache = new Dictionary<int, List<Point>>(); // adapter caching

        private static int count;

        public LineToPointAdapter(Line line)
        {
            var has = line.GetHashCode();
            if (cache.ContainsKey(has)) return; // if the code cotaiss the has code, return 

            Console.WriteLine($"{++count} : {line.Start.X}, {line.Start.Y}");

            var points = new List<Point>();

            points.Add(new Point(0, 0));
        }
    }

    internal class Adapter
    {
        private static readonly List<VectorObject> vectorObjects = new List<VectorObject>
        {
            new VectorRectangle(1, 1, 10, 10), // a line to a set of points
            new VectorRectangle(3, 3, 6, 6)
        };

        public static void DrawPoint(Point p)
        {
            Console.WriteLine(".");
        }

        static void Main(string[] args)
        {
            foreach(var obj in vectorObjects) 
            {
                foreach(var i in obj)
                {
                    var adapter = new LineToPointAdapter(i);
                    adapter.ForEach(DrawPoint);
                }
            }
        }
    }
}
