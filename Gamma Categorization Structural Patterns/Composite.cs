using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamma_Categorization_Structural_Patterns
{
    // Objects use other objects fields/prop/members throung inheritance and composition
    // Compoition let us make compund objects 
    // Composite design pattern is used to treat both single and composite objects uniformly ( Foo and Collection<Foo> have common API )
    // def -> a mechanism for treating individual obj and compositions of objects in a uniform manner
    // Some composed and singular objects need similar behaviors
    // C# has special support for the enumeration concept
    // A single obj can masquerade as a collection with yield return this;

    public class GraphicObject
    {
        public virtual string Name { get; set; } = "Group";
        public string? Color;

        private Lazy<List<GraphicObject>> children = new Lazy<List<GraphicObject>>();
        public List<GraphicObject> Children => children.Value;

        private void Print(StringBuilder sb, int depth)
        {
            sb.Append(new string('*', depth)).Append(string.IsNullOrWhiteSpace(Color) ? String.Empty : $"{Color}").Append(Name);
            foreach (var child in Children)
            {
                child.Print(sb, depth+1);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Print(sb, 0);
            return sb.ToString();
        }
    }

    public class Circles : GraphicObject 
    {
        public override string Name => "Circle";
    } 

    public class Square : GraphicObject 
    {
        public override string Name => "Sqaure";
    }
    internal class Composite
    {
        static void Main(string[] args)
        {
            var drawing = new GraphicObject { Name = "My Drawing" };
            drawing.Children.Add(new Square { Color = "Red" });
            drawing.Children.Add(new Circles { Color = "Yellow" });

            var group = new GraphicObject();
            group.Children.Add(new Square { Color = "Blue" });
            group.Children.Add(new Circles { Color = "Blue" });
            drawing.Children.Add(group);

            Console.WriteLine(drawing);
        }
    }

    public static class ExtensionMethods
    {
        public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other)
        {
            if (ReferenceEquals(self, other)) return;
            foreach (var from in self)
                foreach(var to in other)
                {
                    from.Out.Add(to);
                    to.In.Add(from);
                }
        }
    }

    public class Neuron : IEnumerable<Neuron> 
    {
        public float value;
        public List<Neuron> In, Out;

        public IEnumerable<Neuron> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator<Neuron> IEnumerable<Neuron>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class NeuronLayer : Collection<Neuron>
    {

    }

    static class Program
    {
        static void Main(String[] args)
        {
            var neuron1 = new Neuron();
            var neuron2 = new Neuron(); 

            neuron1.ConnectTo(neuron2); // 1 method

            var layer1 = new NeuronLayer();
            var layer2 = new NeuronLayer();

            // 4 methods

            neuron1.ConnectTo(neuron2);
        }
    }
}
