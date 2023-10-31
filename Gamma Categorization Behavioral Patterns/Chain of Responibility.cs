using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Gamma_Categorization_Behavioral_Patterns
{
    // You click a button -> Button handles it, stopss further processing -> Underlying group box -> Underlying window
    // def -> a chain of components who all get a change to process a command or query, optionally,
    //        having default processing implementation and an ability to terminate the processing chain
    // Command Query Separation -> having separate means of sending commands and queries to direct field access
    // Enlist objects in the chain, possibly controlling their order
    // Object removal from chain ( Dispose() )

    public class Creature
    {
        public string Name;
        public int Attack, Defense;

        public Creature(string name, int attack, int defense) 
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Attack = attack;
            Defense = defense;
        }

        public override string ToString()
        {
            return $"{Name}, {Attack}, {Defense}";
        }
    }

    public class CreatureModifier
    {
        protected Creature creature;
        protected CreatureModifier next; // linked list

        public CreatureModifier(Creature creature) 
        {
            this.creature = creature ?? throw new ArgumentNullException(nameof(creature));
        }

        public void Add(CreatureModifier cm)
        {
            if(next != null) next.Add(cm);
            else next = cm;
        }

        public virtual void Handle() => next?.Handle();
    }

    public class DoubleModifier : CreatureModifier
    {
        public DoubleModifier(Creature creature) : base(creature) { }

        public override void Handle() 
        {
            Console.WriteLine(creature.Name);
            creature.Attack *= 2;
            base.Handle();
        }
    }

    public class Game // exemple using Mediator design pattern
    {
        public event EventHandler<Query> Queries;

        public void PerformQuery(object sender, Query q)
        {
            Queries?.Invoke(sender, q);
        }
    }

    public class Query
    {
        public string CreatureName;

        public enum Argument
        {
            Attack, Defense
        }

        public Argument WhatToQuery;
        public int Value;

        public Query(string creatureName, Argument whatToQuery, int value)
        {
            CreatureName = creatureName ?? throw new ArgumentNullException(nameof(creatureName));
            WhatToQuery = whatToQuery;
            Value = value;
        }
    }

    public class Creatures
    {
        private Game game; // reference of game
        public string Name;
        private int attack, defense;

        public Creatures(Game game, string name, int attack, int defense)
        {
            this.game = game;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            this.attack = attack;
            this.defense = defense;
        }

        public int Attack
        {
            get
            {
                var q = new Query(Name, Query.Argument.Attack, attack);
                game.PerformQuery(this, q); // q.Value
                return q.Value;
            }
        } // accessors
        public int Defense
        {
            get
            {
                var q = new Query(Name, Query.Argument.Defense, Defense);
                game.PerformQuery(this, q); // q.Value
                return q.Value;
            }
        }

        public static implicit operator Creatures(Creature v)
        {
            throw new NotImplementedException();
        }
    }
    public abstract class CreatureModifier2 : IDisposable 
    {
        protected Game game;
        protected Creatures creatures;

        protected CreatureModifier2(Game game, Creature creatures) 
        {
            this.game = game ?? throw new ArgumentNullException(nameof(game));
            this.creatures = creatures ?? throw new ArgumentNullException(nameof(creatures));
            game.Queries += Handle;
        }

        protected CreatureModifier2(Game game, Creatures creatures)
        {
            this.game = game;
            this.creatures = creatures;
        }

        protected abstract void Handle(object sender, Query q);

        public void Dispose() 
        {
            game.Queries -= Handle;
        }
    }

    public class DoubleAttackModifier2 : CreatureModifier2
    {
        public DoubleAttackModifier2(Game game, Creatures creatures) : base(game, creatures) { }

        protected override void Handle(object sender, Query q)
        {
            if(q.CreatureName == creatures.Name && q.WhatToQuery == Query.Argument.Attack)
            {
                q.Value *= 2;
            }
        }
    }

    internal class Chain_of_Responibility
    {
        public static void Main1(string[] args)
        {
            var goblin = new Creature("Goblin", 2, 2);
            Console.WriteLine(goblin);
        }
    }
}
