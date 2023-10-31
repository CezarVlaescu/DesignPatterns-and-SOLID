using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamma_Categorization_Behavioral_Patterns
{
    // An object or system goes through changes
    // def -> a token/handle representing the system state. Lets us roll back to the state when the token was generated. May or may not directly expose state information
    // A memento is not required to expose directly the state to which it reverts the system

    public class Memento // token
    {
        public int Balance { get; }

        public Memento(int balance)
        {
            Balance = balance;
        }
    }

    public class BankAccount2
    {
        private int balance;

        public BankAccount2(int balance)
        {
            this.balance = balance;
        }

        public Memento Deposit(int amount)
        {
            balance += amount;
            return new Memento(balance);
        }

        public void Restore(Memento m)
        {
            balance = m.Balance;
        }

        public override string ToString()
        {
            return $"{nameof(balance)}: {balance}";
        }
    }

    public class Demo5
    {
        static void Main(string[] args)
        {
            var ba = new BankAccount2(100);
            var m1 = ba.Deposit(50); // 150
            var m2 = ba.Deposit(25); // 175
            Console.WriteLine(ba);

            // restore to m1
            ba.Restore(m1);
            Console.WriteLine(ba);

            ba.Restore(m2);
            Console.WriteLine(ba);
        }
    }
}
