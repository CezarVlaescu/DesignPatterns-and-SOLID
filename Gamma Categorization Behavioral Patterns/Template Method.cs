using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamma_Categorization_Behavioral_Patterns
{
    // Algo can be decomposed into common parts + specific 
    // Strategy pattern does this through composition ( high level algo use an interface, concrete implementations implement the interface )
    // def -> allows us to define the skeleton of the algo, with concrete implementations defined in subclasses
    // Define an algo at a high level, define constituent parts as abstract methods/prop, inherit the algo class providing necessary overrides

    public abstract class Games
    {
        public void Run()
        {
            Start();
            while (!HaveWinner)
                TakeTurn();
            Console.WriteLine($"Player {WinningPlayer} wins.");
        }

        protected abstract void Start();
        protected abstract bool HaveWinner { get; }
        protected abstract void TakeTurn();
        protected abstract int WinningPlayer { get; }

        protected int currentPlayer;
        protected readonly int numberOfPlayers;

        public Games(int numberOfPlayers)
        {
            this.numberOfPlayers = numberOfPlayers;
        }
    }

    // simulate a game of chess
    public class Chess : Games
    {
        public Chess() : base(2)
        {
        }

        protected override void Start()
        {
            Console.WriteLine($"Starting a game of chess with {numberOfPlayers} players.");
        }

        protected override bool HaveWinner => turn == maxTurns;

        protected override void TakeTurn()
        {
            Console.WriteLine($"Turn {turn++} taken by player {currentPlayer}.");
            currentPlayer = (currentPlayer + 1) % numberOfPlayers;
        }

        protected override int WinningPlayer => currentPlayer;

        private int maxTurns = 10;
        private int turn = 1;
    }

    public class Demo2
    {
        static void Main(string[] args)
        {
            var chess = new Chess();
            chess.Run();
        }
    }
}
