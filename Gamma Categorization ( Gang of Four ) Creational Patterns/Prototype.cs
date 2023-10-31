using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gamma_Categorization___Gang_of_Four___Creational_Patterns
{
    // Complicated objects aren't design from scratch, they reiterate existing designs
    // An existing object design is a Prototype ( ex proto from HTML )
    // Deep copy -> copy all the object that replacate the state of references, and don't effect the big object ( proto )
    // Def -> a partially or fully initialized object that you copy and make use of.

    public static class ExtensionMethods
    {
        public static T DeepCopy<T>(this T self)
        {
            var stream = new MemoryStream();
            var formater = new BinaryFormatter();
            formater.Serialize(stream, self);
            stream.Seek(0, SeekOrigin.Begin);
            object copy = formater.Deserialize(stream);
            stream.Close();
            return (T)copy;
        }

        public static T DeepCopyXml <T>(this T self) // instand of [Serializable]
        {
            using(var stream = new MemoryStream())
            {
                var s = new XmlSerializer(typeof(T));
                s.Serialize(stream, self);
                stream.Position = 0;
                return (T) s.Deserialize(stream);
            }
        }
    }
    public class Prototype 
    {
        public string[] Names;
        public Address Address;

        public Prototype() { }

        public Prototype(string[] names, Address address) 
        {
            Names = names;
            Address = address;
        }

        public Prototype(Prototype other)
        {
            Names = other.Names;
            Address = new Address(other.Address);
        }

        public override string ToString()
        {
            return $"{Names}, {Address}";
        }
    }
    public class Address 
    {
        private string StreetName;
        public int HouseNumber;

        public Address() { }

        public Address(string streetName, int houseNumber)
        {
            StreetName = streetName;
            HouseNumber = houseNumber;
        }

        public Address(Address address)
        {
            StreetName = address.StreetName;
            HouseNumber = address.HouseNumber;
        }

        public override string ToString()
        {
            return $"{StreetName} {HouseNumber}";
        }

    }

    static class Program
    { 
        static void Main(string[] args) 
        {
            var jhon = new Prototype(new[] { "Jhon", "Smith" }, new Address("London Road", 123));
            var jane = jhon.DeepCopyXml();
            jane.Names[0] = "Jane";
            jane.Address.HouseNumber = 321;

            Console.WriteLine(jhon);
            Console.WriteLine(jane);
        }
    }
}
