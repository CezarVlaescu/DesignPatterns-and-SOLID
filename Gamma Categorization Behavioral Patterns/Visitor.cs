using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamma_Categorization_Behavioral_Patterns
{
    // Need to define a new operation on an entire class hierarchy, need access to non-common aspects of classes in the hierarchy, create an external component to handle rendering
    // def -> a pattern where a component(visitor) is allowed to traverse the entire inheritance hierarchy. Implemented by propagating a single visit() method throughout the entire hierarchy.
    // Dispatch -> single dispatch ( depends on name of requests and type of receiver ) and double dispatch ( depends on name of request and type of two receivers )
    // Propagate an accept(Visitor v) method throughout the entire hierarchy, create a visitor with Visit(Foo), Visit(Bar) for each element of the hierarchy, each accept() simply calls visitor.Visit(this),
    // using dynamic we can invoke right overload based on argument type alone ( dynamic dispach )

    // Instrusive Visitor
    public abstract class Expression
    {
        // adding a new operation
        public abstract void Print(StringBuilder sb);
    }

    public class DoubleExpression : Expression
    {
        private double value;

        public DoubleExpression(double value)
        {
            this.value = value;
        }

        public override void Print(StringBuilder sb)
        {
            sb.Append(value);
        }
    }

    public class AdditionExpression : Expression
    {
        private Expression left, right;

        public AdditionExpression(Expression left, Expression right)
        {
            this.left = left ?? throw new ArgumentNullException(paramName: nameof(left));
            this.right = right ?? throw new ArgumentNullException(paramName: nameof(right));
        }

        public override void Print(StringBuilder sb)
        {
            sb.Append(value: "(");
            left.Print(sb);
            sb.Append(value: "+");
            right.Print(sb);
            sb.Append(value: ")");
        }
    }

    // Reflective Visitor

    using DictType = Dictionary<Type, Action<Expression, StringBuilder>>;

    public abstract class Expression1
    {
    }

    public class DoubleExpression1 : Expression1
    {
        public double Value;

        public DoubleExpression1(double value)
        {
            Value = value;
        }
    }

    public class AdditionExpression1 : Expression1
    {
        public Expression Left;
        public Expression Right;

        public AdditionExpression1(Expression left, Expression right)
        {
            Left = left ?? throw new ArgumentNullException(paramName: nameof(left));
            Right = right ?? throw new ArgumentNullException(paramName: nameof(right));
        }
    }

    public static class ExpressionPrinter1
    {
        private static DictType actions = new DictType
        {
            [typeof(DoubleExpression)] = (e, sb) =>
            {
                var de = (DoubleExpression)e;
                sb.Append(de.Value);
            },
            [typeof(AdditionExpression)] = (e, sb) =>
            {
                var ae = (AdditionExpression)e;
                sb.Append("(");
                Print(ae.Left, sb);
                sb.Append("+");
                Print(ae.Right, sb);
                sb.Append(")");
            }
        };

        public static void Print2(Expression e, StringBuilder sb)
        {
            actions[e.GetType()](e, sb);
        }

        public static void Print(Expression e, StringBuilder sb)
        {
            if (e is DoubleExpression de)
            {
                sb.Append(de.Value);
            }
            else
            if (e is AdditionExpression ae)
            {
                sb.Append("(");
                Print(ae.Left, sb);
                sb.Append("+");
                Print(ae.Right, sb);
                sb.Append(")");
            }
            // breaks open-closed principle
            // will work incorrectly on missing case
        }
    }

    public class Demo9
    {
        private static void Main(string[] args)
        {
            var e = new AdditionExpression(
              left: new DoubleExpression(1),
              right: new AdditionExpression(
                left: new DoubleExpression(2),
                right: new DoubleExpression(3)));
            var sb = new StringBuilder();
            e.Print(sb);
            Console.WriteLine(sb);

            // what is more likely: new type o rnew operation
        }
    }
}
