using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using NUnit.Framework;

namespace Gamma_Categorization___Gang_of_Four___Creational_Patterns
{
    // For some components it only make sense to have one in the system : database repository, object factory
    // The constructor call is expensive, we provide everyone with the same instance
    // Need to take care of lazy instatiation and thread safe
    // Def -> a component which is instantiated only once.

    public interface IDatabase
    {
        int GetPopulation(string name);
    }

    public class SingletoneDatabase : IDatabase
    {
        private Dictionary<string, int> capitals;
        private static int instanceCount;

        public static int Count => instanceCount;

        private SingletoneDatabase()
        {
            instanceCount++;
            Console.WriteLine("Initialize database");
            capitals = File.ReadAllLines(Path.Combine(new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName, "text.txt"))
                .Batch(2).
                ToDictionary(
                l => l.ElementAt(0).Trim(), 
                l => int.Parse(l.ElementAt(1))
            );
        }

        public int GetPopulation(string name) 
        {
            return capitals[name];
        }

        private static Lazy<SingletoneDatabase> instance = new Lazy<SingletoneDatabase>(() => new SingletoneDatabase());

        public static SingletoneDatabase Instance => instance.Value;
    }

    public class SingletonRecordFinder
    {

    }

    public class ConfigureableRecordFinder
    {
        private readonly IDatabase database;

        public ConfigureableRecordFinder(IDatabase database)
        {
            this.database = database;
        }

        public int GetTotalPopulation(IEnumerable<string> names)
        {
            int result = 0;
            foreach (string name in names)
            {
                result += SingletoneDatabase.Instance.GetPopulation(name);
            }
            return result;
        }
    }

    public class DummyDatabase : IDatabase
    {
        public int GetPopulation(string name)
        {
            return new Dictionary<string, int>
            {
                ["alpha"] = 1,
                ["beta"] = 2,
                ["gamma"] = 3
            }[name];
        }
    }     

    [TestFixture]
    public class SingletoneTests // costs a lot of resources, hardcode the reference ( problem with Singleton )
    {
        [Test]
        public void IsSingletonTest()
        {
            var db = SingletoneDatabase.Instance;
            var db2 = SingletoneDatabase.Instance;
            Assert.That(db, Is.SameAs(db2));
            Assert.That(SingletoneDatabase.Count, Is.EqualTo(1));
        }

        /*public void SingletonTotalPopulationTest()
        {
            var rf = new SingletonRecordFinder();
            var names = new[] { "Seoul", "Mexico City" };
            int tp = rf.GetTotalPopulation(names);
            Assert.That(tp, Is.EqualTo(17500000 + 17400000));
        }*/

        public void ConfigurablePopulationTests()
        {
            var rf = new ConfigureableRecordFinder(new DummyDatabase());
            var names = new[] { "alpha", "gamma" };
            int tp = rf.GetTotalPopulation(names);
            Assert.That(tp, Is.EqualTo(4));
        }
    }

    public static class Programs
    {
        static void Main(string[] args)
        {
            var db = SingletoneDatabase.Instance;
            var city = "Tokyo";
            Console.Write(city, db.GetPopulation(city));
        }
    }
}
