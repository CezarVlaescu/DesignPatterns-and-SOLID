using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SOLID_Principles
{
    public enum Color
    {
        Red, Green, Blue, Magenta
    }

    public enum Size
    {
        Small, Medium, Large
    }

    // Classes should be open for extensions ( extend to make new things like this exemple with more filters for color, for size etc. )
    // but closed for modification ( should not come back to editing the code )
    public class Open_Close_Principle
    {
        public string Name;
        public Color Color;
        public Size Size;

        public Open_Close_Principle(string name, Color color, Size size)
        {
            if(name == null) throw new ArgumentNullException(paramName: nameof(name));

            Name = name;
            Color = color;
            Size = size;
        }

        public class ProductFilter
        {
            public static IEnumerable<Open_Close_Principle> FilterBySize(IEnumerable<Open_Close_Principle> products, Size size) // one filter
            {
                foreach(var p  in products)
                {
                    if(p.Size == size) yield return p;
                }
            }

            public static IEnumerable<Open_Close_Principle> FilterByColor(IEnumerable<Open_Close_Principle> products, Color color) // two filter
            {
                foreach(var p in products)
                {
                    if(p.Color == color) yield return p;
                }
            }
        }

        public interface ISpecification<T> // this interfaces responds to close for modification part of principle, we are using instand of coming back to code 
        {
            bool IsSatisfied(T t);
        }

        public interface IFilter<T>
        {
            IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
        }

        public class ColorSpecificaiton : ISpecification<Open_Close_Principle> // we can go for Size too 
        {
            private Color _color;

            public ColorSpecificaiton(Color color)
            {
                _color = color;
            }

            public bool IsSatisfied(Open_Close_Principle item)
            {
                return item.Color == _color;
            }
        }

        public class BetterFillter : IFilter<Open_Close_Principle>
        {
            public IEnumerable<Open_Close_Principle> Filter (IEnumerable<Open_Close_Principle> items, ISpecification<Open_Close_Principle> spec)
            {
                foreach(var i in items)
                {
                    if(spec.IsSatisfied(i)) yield return i;
                }
            }
        }

        public class Demo
        {
            public void Main(string[] args)
            {
                var apple = new Open_Close_Principle("apple", Color.Red, Size.Small);
                var tree = new Open_Close_Principle("tree", Color.Blue, Size.Medium);

                Open_Close_Principle[] products = { apple, tree };

                var pf = new ProductFilter();

                var bf = new BetterFillter();
                Console.WriteLine("Green products(new): ");
                foreach(var p in bf.Filter(products, new ColorSpecificaiton(Color.Green)))
                {
                    Console.WriteLine(p.ToString());
                }

            }
        }
    }
}
