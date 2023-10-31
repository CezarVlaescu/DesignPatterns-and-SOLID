using Autofac;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamma_Categorization_Structural_Patterns
{
    // Connection of components together throungh abstractions
    // Avoids the Cartesian product complexity explosion
    // def -> A mechanism that decouples an interface from an implementation hierarchy.
    // Connecting the main object to different implementation on the rendering mechanism (other parts of the system )
    // Decouple abstraction from implementation
    // Both can exist as hierarchies
    // Strong form of encapsulation

    public interface IRenderer
    {
        void RenderCircle(float radius);
    }

    public class VectorRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            Console.WriteLine(radius);
        }
    }

    public class RasterRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            Console.WriteLine(radius);
        }
    }

    public abstract class Bridge
    {
        protected IRenderer renderer;

        protected Bridge(IRenderer renderer) 
        {
            this.renderer = renderer;
        }

        public abstract void Draw();
        public abstract void Resize(float factor);
    }

    public class Circle : Bridge
    {
        private float radius;

        public Circle(IRenderer renderer, float radius) : base(renderer)
        {
            this.radius = radius;
        }

        public override void Draw() 
        {
            renderer.RenderCircle(radius);
        }

        public override void Resize(float factor) 
        {
            radius *= factor;
        }
    }

    class Demo
    {
        static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<VectorRenderer>().As<IRenderer>().SingleInstance();
            containerBuilder.Register((c, p) => new Circle(c.Resolve<IRenderer>(), p.Positional<float>(0)));

            using(var c = containerBuilder.Build()) 
            {
                var circle = c.Resolve<Circle>(new PositionalParameter(0, 0.5f));
                circle.Draw();
                circle.Resize(2);
            }
        }
    }
}
