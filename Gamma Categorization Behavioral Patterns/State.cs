using Microsoft.Azure.KeyVault.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamma_Categorization_Behavioral_Patterns
{
    // Consider an ordinary telephone, if it's ringing or you want to make a call, you can pick it up, phone must be off the hook to talk, if you try calling someone, and is busy you put handset down
    // Changes in state can be explicit or in response to event ( Observer pattern )
    // def -> a pattern in which the obj behavior is determined by it's state. An obj transitions from one state to another ( something needs to trigger a transition )
    // State machine -> a formalized construct which manages state and transitions
    // Given sufficient complexity, it pays to formally define possible states and events/triggers
    // Can define state entry/exit behaviors, action when a particular event causes a transition, guard codition enabling/disabling a transition

    public class Switch
    {
        public State State = new OffState();
        public void On() { State.On(this); }
        public void Off() { State.Off(this); }   
    }

    public abstract class State
    {
        public virtual void On(Switch sw)
        {
            Console.WriteLine("Light is already on");
        }
        public virtual void Off(Switch sw) 
        {
            Console.WriteLine("Light is already off");
        }
    }

    public class OnState : State
    {
        public OnState()
        {
            Console.WriteLine("Light turned on");
        }
    }

    public class OffState : State
    {
        public OffState()
        {
            Console.WriteLine("Light turned off");
        }

        public override void On(Switch sw)
        {
            Console.WriteLine("Turning light on....");
            sw.State = new OnState();
        }
    }

    // Handmade state machine

    public enum States
    {
        OffHook,
        Connecting,
        Connected,
        OnHold
    }

    public enum MyEnum
    {
        CallDialed,
        HungUp,
        CallConnected,
        PlaceOnHold,
        LeftMessage
    }

    internal class Programse
    {
        public static void Main(string[] args)
        {
            var ls = new Switch();
            ls.On();
            ls.Off();
            ls.Off();

            /*private static Dictionary<State, List<(Trigger, State)>> rules
      = new Dictionary<State, List<(Trigger, State)>>
      {
          [State.OffHook] = new List<(Trigger, State)>
        {
          (Trigger.CallDialed, State.Connecting)
        },
          [State.Connecting] = new List<(Trigger, State)>
        {
          (Trigger.HungUp, State.OffHook),
          (Trigger.CallConnected, State.Connected)
        },
          [State.Connected] = new List<(Trigger, State)>
        {
          (Trigger.LeftMessage, State.OffHook),
          (Trigger.HungUp, State.OffHook),
          (Trigger.PlacedOnHold, State.OnHold)
        },
          [State.OnHold] = new List<(Trigger, State)>
        {
          (Trigger.TakenOffHold, State.Connected),
          (Trigger.HungUp, State.OffHook)
        }
      };*/
    }

    }
}
