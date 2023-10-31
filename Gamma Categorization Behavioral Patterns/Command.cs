using DocumentFormat.OpenXml.Office2016.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamma_Categorization_Behavioral_Patterns
{
    // C# statements : cannot undo a field/prop assignment, cannot directly serialize a sequence of actions(calls)
    // Want an object that represents an operation : X should change it's proper Y to Z, X should do W()
    // def -> an object which represents an instruction to perform a particular action. Contains all the informations necessary for the action to be taken.
    // Encapsulate all details of an operation in a seperate object
    // Define instruction for applying the command, optionally define instruction for undoing the command, can create composite commands ( macros )

    public class BankAccount
    {
        private int balance;
        private int overtheLimit = -500;

        public void Deposit(int amount)
        {
            balance += amount;
        }

        public void Withdraw(int amount) 
        {
            if(balance - amount >= overtheLimit)
            {
                balance -= amount;
            }
        }
    }

    public interface ICommand
    {
        void Call();
        void Undo();
    }

    public class BankAccountCommand : ICommand
    {
        private BankAccount account;

        public enum Action
        {
            Deposit, Withdraw
        }

        private Action action;
        private int amount;

        public BankAccountCommand(BankAccount account, Action action, int amount)
        {
            this.account = account ?? throw new ArgumentNullException(nameof(account));
            this.action = action;
            this.amount = amount;
        }

        public void Call() 
        {
            switch(action)
            {
                case Action.Deposit:
                    account.Deposit(amount); break;
                case Action.Withdraw: 
                    account.Withdraw(amount); break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void Undo()
        {
            switch(action)
            {
                case Action.Deposit:
                    account.Withdraw(amount); break;
                case Action.Withdraw:
                    account.Deposit(amount); break;
                default:
                    throw new NotImplementedException();
            }
        }
    }

    // Composite command ( transfer a -> b )

    public class CompositeBankAccountCommand : List<BankAccountCommand>, ICommand
    {
        void ICommand.Call()
        {
            ForEach(cmd => cmd.Call()); 
        }

        void ICommand.Undo()
        {
            foreach(var cmd in ((IEnumerable<BankAccountCommand>) this).Reverse())
            {
                if (cmd.Equals(2))
                {
                    cmd.Undo();
                }
            }
        }

        public bool Success 
        {
            get { return this.All(cmd => cmd.Equals(0)); } 
            set
            {
                foreach (var cmd in this) return;
                    //cmd = value;
            }
        }
    }
    internal class Command
    {
        static void Main2(string[] args)
        {
            var ba = new BankAccount();
            var commands = new List<BankAccountCommand>
            {
                new BankAccountCommand(ba, BankAccountCommand.Action.Deposit, 100),
                new BankAccountCommand(ba, BankAccountCommand.Action.Withdraw, 100)
            };

            foreach (var command in commands)
            {
                command.Call();
            }

            Console.WriteLine(ba);

            foreach(var c in Enumerable.Reverse(commands))
            {
                c.Undo();
            }

            Console.WriteLine(ba);
        }
    }
}
