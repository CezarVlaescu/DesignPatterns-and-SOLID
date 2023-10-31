using JetBrains.dotMemoryUnit;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamma_Categorization_Structural_Patterns
{
    // Space optimization, avoid redundancy when storing data
    // def -> a space optimization technique that let us use less memory by storing externally the data associated with similar objects ( like MMORPG )
    // Store common data externally
    // Define the idea of ranges on homegeneous collections and store data related to those ranges
    

    public class User
    {
        private string FullName;

        public User(string fullname) 
        {
            this.FullName = fullname;
        }
    }

    public class User2 // and create a TestUser too ( Flyweight class )
    {
        static List<string> strings = new List<string>();
        private int[] names;
        public User2(string fullName) 
        {
            int getOrAdd(string s)
            {
                int idx = strings.IndexOf(s);
                if(idx != -1) return idx;
                else
                {
                    strings.Add(s);
                    return strings.Count - 1;
                }
            }

            names = fullName.Split(',').Select(getOrAdd).ToArray();
        }

        public string FullName => string.Join(",", names.Select(i => strings[i]));
    }

    [TestFixture]
    internal class Flyweight
    {
        static void Main(string[] args)
        {

        }
        [Test]
        public void TestUser() 
        {
            var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());

            var users = new List<User>();

            foreach(var firstName in firstNames)
                foreach(var lastName in lastNames)
                    users.Add(new User($"{firstName} {lastName}"));

            ForceGC();

            dotMemory.Check(memory =>
            {
                Console.WriteLine(memory.SizeInBytes);
            });
            
        }

        private void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private string RandomString()
        {
            Random random = new Random();
            return new string(Enumerable.Range(0, 10)
                .Select(i => (char)('a' + random.Next(26)))
                .ToArray());
        }
    }

    // text formatting

    public class FormattedText
    {
        private readonly string plainText;
        private bool[] capitalize;

        public FormattedText(string plainText)
        {
            this.plainText = plainText;
            capitalize = new bool[plainText.Length];    
        }

        public void Capitalize(int start, int end)
        {
            for(int i = start; i < end; i++)
            {
                capitalize[i] = true;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for(var i = 0; i < plainText.Length; i++)
            {
                var c = plainText[i];
                sb.Append(capitalize[i] ? char.ToUpper(c) : c);
            }
            return sb.ToString();
        }
    }

    public class BetterFormattedText
    {
        private string plainText;
        private List<TextRange> formatting = new List<TextRange>();

        public BetterFormattedText(string plainText)
        {
            this.plainText = plainText;
        }

        public TextRange GetRange(int start, int end)
        {
            var range = new TextRange { Start = start, End = end };
            formatting.Add(range);
            return range;
        }

        public class TextRange
        {
            public int Start, End;
            public bool Capitalize, Bold, Italic;

            public bool Convers(int position)
            {
                return position >= Start && position <= End;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for(var i = 0; i < plainText.Length; i++)
            {
                var c = plainText[i];   
                foreach(var range in formatting)
                {
                    if(range.Convers(i) && range.Capitalize)
                        c = char.ToUpper(c);
                }
            }
            return sb.ToString();

        }
    }
}
