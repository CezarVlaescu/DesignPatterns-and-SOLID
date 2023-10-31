using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID_Principles
{
    // A class is responsable for one thing and has only one reason to change
    // Separation of concerns (SoC) -> diff classes handling diff, independent tasks/problems
    public class Journal
    {
        private readonly List<string> entries = new List<string>();

        private static int count = 0;

        public int AddEntry(string entry)
        {
            entries.Add($"{++count}: {entry}");
            return count; // memento
        }

        public void RemoveEntry(int index)
        {
            entries.RemoveAt(index);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, entries);
        }

        // IF we keep adding methods then there is a violation of S principle
    }
    internal class Single_Responsability_Principle
    {
        static void Main(string[] args)
        {
            Journal journal = new Journal();
            journal.AddEntry("I cry");
            journal.AddEntry("i eat");
            Console.WriteLine(journal);
        }
    }
}
