using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamma_Categorization_Creational_Patterns
{
    // Object creation logic becomes too convoluted, constructor not descriptive
    // Cannot overload with same sets of args with diff names
    // Object creation can be outsourced to a separate function ( a separate function ), that may exist a separate class ( Factory ), can create hierarchy of factories ( Abstract Factory )
    // Def -> a component responsible solely for the wholesale creation of objects

    public class Factory
    {
        private double x, y;

        private Factory(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public override string ToString() 
        {
            return $"{nameof(x)} : {x}, {nameof(y)} : {y}";
        }

        public static Factory Origin => new Factory(0.0, 0.0);

        public static class PointFactory
        {
            // Factory method
            public static Factory NewCartesianPoint(double x, double y)
            {
                return new Factory(x, y);
            }

            public static Factory NewPolarPoint(double rho, double theta)
            {
                return new Factory(rho * Math.Cos(theta), rho * Math.Sin(theta));
            }
        }
    }

    public interface IHotDrink
    {
        void Consume();
    }

    internal class Tea : IHotDrink
    {
        public void Consume()
        {
            Console.WriteLine("This tea is nice, I prefer it with milk");
        }
    }

    internal class Coffee : IHotDrink
    {
        public void Consume()
        {
            Console.WriteLine("Thiss coffee is awesome");
        }
    }

    public interface IHotDrinkFactory
    {
        IHotDrink Prepare(int amount); // a factory for interface
    }

    public class AbstractFactory : IHotDrinkFactory // returning abstract classes or interfaces
    {
        public IHotDrink Prepare(int amount)
        {
            Console.WriteLine($"Grind some beans, boil water, pour {amount} ml");
            return new Tea(); // or new Coffee();
        }
    }

    public class HotDrinkMachine
    {
        public enum AvailableDrinks
        {
            Coffee, Tea
        }

        private Dictionary<AvailableDrinks, IHotDrinkFactory> factories = new Dictionary<AvailableDrinks, IHotDrinkFactory> ();

        public HotDrinkMachine()
        {
            foreach(AvailableDrinks drink in Enum.GetValues (typeof(AvailableDrinks)))
            {
                var factory = (IHotDrinkFactory)Activator.CreateInstance(Type.GetType("DesignPatterns " + Enum.GetName(typeof(AvailableDrinks), drink) + "Factory"));
                factories.Add(drink, factory);
            }
        }

        public IHotDrink MakeDrink(AvailableDrinks drink, int amount)
        {
            return factories[drink].Prepare(amount);
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            var point = Factory.PointFactory.NewPolarPoint(1.0, Math.PI / 2);
            Console.WriteLine(point);

            var machine = new HotDrinkMachine();
            var drink = machine.MakeDrink(HotDrinkMachine.AvailableDrinks.Tea, 100);
            drink.Consume();
        }
    }
}
