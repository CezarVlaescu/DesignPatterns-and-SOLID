using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gamma_Categorization_Structural_Patterns
{
    // Adding behavior without altering the class itself
    // Want to augment an object with additional functionality
    // No rewrite or alter existing code ( OCP ), want to keep new functionality separate ( SRP )
    // def -> facilitates the addition of behaviors to individual objects without inheriting from them
    // A decorator keeps the reference to the decorated object, may proxy overcalls
    // In C# exists in a static variation (X<Y<Foo>>)
    

    public class CodeBuilder
    {
        private StringBuilder builder = new StringBuilder();

        public override string ToString()
        {
            return builder.ToString(); 
        }

        public void ObjectData(SerializationInfo info, StreamingContext context)
        {
            ((ISerializable) builder).GetObjectData(info, context);
        }

        public static implicit operator CodeBuilder(string s)
        {
            var msb = new CodeBuilder();
            msb.builder.Append(s);
            return msb;
        }

        // msb += foo

        public static CodeBuilder operator +(CodeBuilder msb, string s)
        {
            msb.builder.Append(s);
            return msb;
        }
        public int EnsureCapacity(int capacity)
        {
            return builder.EnsureCapacity(capacity);
        }

        public CodeBuilder Clear() // this is one of many methods of StringBulder, but addapted to decorator, we change from StringBuilder with CodeBuilder
        {
            builder.Clear();
            return this;
        }
    }

    // dynamic composition decorator

    public abstract class Shape
    {
        public abstract string AsString();
    }

    class Circling : Shape
    {
        private float radius;

        public Circling(float radius)
        {
            this.radius = radius;
        }

        public void Resize(float factor)
        {
            radius *= factor;
        }
        public override string AsString() => $"{radius}";
    }

    class Squaring : Shape
    {
        private float side;
        
        public Squaring(float side)
        {
            this.side = side;
        }

        public override string AsString() => $"{side}";
    }

    public class ColoredShape : Shape // decorator to add color to the circle
    {
        private Shape shape;
        private string color;

        public ColoredShape(Shape shape, string color)
        {
            this.shape = shape;
            this.color = color;
        }

        public override string AsString() => $"{shape.AsString()}, {color}";   
    }

    public class TransparentShape<T> : Shape where T : Shape, new()
    {
        private string transparent;
        private T shape = new T();

        public TransparentShape() : this("opacity") { }

        public TransparentShape(string transparat)
        {
            this.transparent = transparat;
        }

        public override string AsString() => $"{transparent}, {shape.AsString()}";
    }



    internal class Decorator
    {
        static void Main(string[] args)
        {

        }
    }
}
