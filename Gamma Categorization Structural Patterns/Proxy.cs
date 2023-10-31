using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Gamma_Categorization_Structural_Patterns
{
    // A interface for accessing a particular resource
    // Same interface, entirely diff behavior 
    // Communication proxy -> something that substitute a different execution, by invokation local 
    // def -> a class that functions as an interface to a particular resource, that resource may be remote, expensive to construct, or may require logging or some other added functionality
    // Proxy vs Decorator : proxy provides identical interface, decorator typically aggregates what is decorating, proxy not even work with a materialized object
    // To create a proxy simply replicate the existing interface of an object
    // Add diff functionality to the redefined member functions 
    // Diff proxies have diff behaviors


    // protection proxy

    public interface ICar
    {
        void Drive();
    }
    public class Car : ICar
    {
        public void Drive() 
        {
            Console.WriteLine("Driving.....");
        }
    }
    public class Driver
    {
        public int Age { get; set; }

        public Driver(int age) 
        {
            Age = age;
        }
    }
    public class ProxyCar : ICar
    {
        private Driver driver;
        private Car car = new Car();

        public ProxyCar(Driver driver)
        {
            this.driver = driver;
        }

        public void Drive()
        {
            if (driver.Age >= 16) car.Drive();
            else
            {
                Console.WriteLine("too young");
            }

        }
    }

    // property proxy

    public class Property<T> where T : new() 
    {
        private T value;

        public T Value
        {
            get => value;
            set
            {
                if (Equals(this.value, value)) return; this.value = value;
            }
        }

        public Property() : this(default(T))
        {

        }

        public Property(T value)
        {
            this.value = value;
        }

        public static implicit operator T(Property<T> property)
        {
            return property.Value; // int n = p_int
        }

        public static implicit operator Property<T>(T value)
        {
            return new Property<T>(value);  // Property<int> p = 123
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }

    // value proxy ( you want stronger type )

    [DebuggerDisplay("value")]
    public struct Percentage
    {
        private readonly double value;
        internal Percentage(double value)
        {
            this.value = value;
        }

        public static double operator *(double f, Percentage p)
        {
            return f * p.value;
        }
        public static Percentage operator +(Percentage f, Percentage p)
        {
            return new Percentage(f.value + p.value);
        }

        public override string ToString()
        {
            return $"{value * 100}";
        }
    }

    public static class PercentageExtensions
    {
        public static Percentage Percent(this double value)
        {
            return new Percentage(value / 100);
        }

        public static Percentage Percent(this int value) // overload
        {
            return new Percentage(value / 100);
        }
    }

    // composite proxy ( allows to implement a pattern or a construct which call a construct )

    internal class Proxy
    {
        static void Main(string[] args)
        {
            ICar car = new ProxyCar(new Driver(12));
            car.Drive();

            Console.WriteLine(10 * 5.Percent());
            Console.WriteLine(2.Percent() + 3.Percent()); // 5%
        }
    }

}
