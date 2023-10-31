using DocumentFormat.OpenXml.ExtendedProperties;
using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Gamma_Categorization_Behavioral_Patterns
{
    // We need to be informed when certain thing happen ( event ), we want to listen to events and notified when they occur, built into C# with event keyword
    // def -> is an object that wishes to be informed about events happening in the system, entity that generating the event
    // Special care must be taken to prevent issues in multithreaded scenarios 
    // IObserver are used in stream processing (pipe)

    // Observer via event

    public class FallsIllEventArgs
    {
        public string Address;
    }

    public class Persons
    {
        public void CatchCold()
        {
            PersonChanged?.Invoke(this, new FallsIllEventArgs { Address = "London"} );
        }
        public event EventHandler<FallsIllEventArgs> PersonChanged;
    }

    // Observer via interfaces

    public class Event
    {

    }

    public class FallsIllEvent : Event 
    {
        public string address;
    }

    public class Patient : IObservable<Event>
    {
        private readonly HashSet<Subscription> subscriptions = new HashSet<Subscription>();

        IDisposable IObservable<Event>.Subscribe(IObserver<Event> observer)
        {
            var subscription = new Subscription(this, observer);
            subscriptions.Add(subscription);
            return subscription;
        }

        public void FallIll()
        {
            foreach(var s in subscriptions)
            {
                s.Observer.OnNext(new FallsIllEvent { address = "London"} );
            }
        }

        private class Subscription : IDisposable
        {
            private readonly Patient patient;
            public readonly IObserver<Event> Observer;

            public Subscription(Patient patient, IObserver<Event> observer) 
            {
                this.patient = patient;
                Observer = observer;
            }
            public void Dispose()
            {
                patient.subscriptions.Remove(this);
            }
        }
    }

    public class Prog : IObserver<Event>
    {
        static void Main11(string[] args)
        {
            new Prog();
        }

        public Prog()
        {
            var patient = new Patient();
            //IDisposable sub = patient.Subscribe(this);

            patient.FallIll();
            patient.OfType<FallsIllEvent>().Subscribe(args => Console.Write($"{args}"));
        }
        void IObserver<Event>.OnCompleted() {}

        void IObserver<Event>.OnError(Exception error){}

        void IObserver<Event>.OnNext(Event value)
        {
            if(value is FallsIllEvent args)
            {
                Console.WriteLine($"A doctor is required at {args.address}");
            }
        }
    }

    // Observer via Collections
    // Bidirectional Observer

    public class Product : INotifyPropertyChanged
    {
        private string name;

        public string Name
        {
            get => name;
            set
            {
                if (value == name) return; // critical
                name = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
          [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"Product: {Name}";
        }
    }

    public class Window : INotifyPropertyChanged
    {
        private string productName;

        public string ProductName
        {
            get => productName;
            set
            {
                if (value == productName) return; // critical
                productName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Window(Product product)
        {
            ProductName = product.Name;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
          [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"Window: {ProductName}";
        }
    }

    public sealed class BidirectionalBinding : IDisposable
    {
        private bool disposed;

        public BidirectionalBinding(
          INotifyPropertyChanged first,
          Expression<Func<object>> firstProperty,
          INotifyPropertyChanged second,
          Expression<Func<object>> secondProperty)
        {
            if (firstProperty.Body is MemberExpression firstExpr
                && secondProperty.Body is MemberExpression secondExpr)
            {
                if (firstExpr.Member is PropertyInfo firstProp
                    && secondExpr.Member is PropertyInfo secondProp)
                {
                    first.PropertyChanged += (sender, args) =>
                    {
                        if (!disposed)
                        {
                            secondProp.SetValue(second, firstProp.GetValue(first));
                        }
                    };
                    second.PropertyChanged += (sender, args) =>
                    {
                        if (!disposed)
                        {
                            firstProp.SetValue(first, secondProp.GetValue(second));
                        }
                    };
                }
            }
        }

        public void Dispose()
        {
            disposed = true;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var product = new Product { Name = "Book" };
            var window = new Window(product);

            // want to ensure that when product name changes
            // in one component, it also changes in another

            // product.PropertyChanged += (sender, eventArgs) =>
            // {
            //   if (eventArgs.PropertyName == "Name")
            //   {
            //     Console.WriteLine("Name changed in Product");
            //     window.ProductName = product.Name;
            //   }
            // };
            //
            // window.PropertyChanged += (sender, eventArgs) =>
            // {
            //   if (eventArgs.PropertyName == "ProductName")
            //   {
            //     Console.WriteLine("Name changed in Window");
            //     product.Name = window.ProductName;
            //   }
            // };

            using var binding = new BidirectionalBinding(
              product,
              () => product.Name,
              window,
              () => window.ProductName);

            // there is no infinite loop because of
            // self-assignment guard
            product.Name = "Table";
            window.ProductName = "Chair";

            Console.WriteLine(product);
            Console.WriteLine(window);
        }
    }

    internal class Observer
    {
        static void Main10(string[] args)
        {
            var person = new Persons();
            person.PersonChanged += CallDoctor;
            person.CatchCold();
        }

        private static void CallDoctor(object sender, FallsIllEventArgs e)
        {
            Console.WriteLine($"A doctor has been called to {e.Address}");
        }
    }
}
