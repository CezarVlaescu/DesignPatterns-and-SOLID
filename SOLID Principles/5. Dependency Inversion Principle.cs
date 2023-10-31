using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID_Principles
{
    // High level parts of the system should not depend to low level parts of the system direclty, should depend on some kind of abstractions
    internal class Dependency_Inversion_Principle
    {
    }

    public enum Relationship
    {
        Parent, Child, Sibling
    }

    public class Person
    {
        public string Name { get; set; }
    }

    public interface IRelationshipBrowser // abstraction, for low level 
    {
        IEnumerable<Person> FindAllChildren(string name);
    }


    public class Relationships : IRelationshipBrowser// low level part of the system
    {
        private List<(Person, Relationship, Person)> _relations 
            = new List<(Person, Relationship, Person)> ();
        
        public void AddParentAndChild(Person parent, Person child)
        {
            _relations.Add((parent, Relationship.Parent, child));
            _relations.Add((child, Relationship.Child, parent));
        }

        //public List<(Person, Relationship, Person)> Relations => _relations;

        public IEnumerable<Person> FindAllChildren(string name) // good approch
        {
            return _relations.Where(x => x.Item1.Name == "John" && x.Item2 == Relationship.Parent).Select(r => r.Item3);            
        }
    }

    public class Reseach
    {
        /*public Reseach(Relationships relationship) // bad approach
        {
            var relations = relationship.Relations;
            foreach (var item in relations.Where(x => x.Item1.Name == "John" && x.Item2 == Relationship.Parent))
            {
                Console.WriteLine(item.Item3.Name);
            }
        }*/

        public Reseach(IRelationshipBrowser browser) // we don't depend on relationships, we depend on interface
        {
            foreach(var p in browser.FindAllChildren("John")) 
            {
                Console.WriteLine(p.Name);
            }
        }
        static void Main(string[] args)
        {
            var parent = new Person { Name = "John" };
            var child = new Person { Name = "Chris" };
            var child2 = new Person { Name = "Marry" };

            var relationships = new Relationships();
            relationships.AddParentAndChild(parent, child);
            relationships.AddParentAndChild(child, child2);

            //new Reseach(relationships);
        }
    }
}
