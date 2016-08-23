using System;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<IEvent> events = new List<IEvent>
            {
                new Event1(),
                new Event2()
            };
            foreach(var @event in events)
            {
                new Listing().Apply(@event);
            }
            Console.ReadKey();
        }
    }

    public class Listing
    {
        public void Apply(IEvent @event)
        {
            Console.WriteLine("IEvent");
            this.Apply(@event as dynamic);
        }

        public void Apply(Event1 @event)
        {
            Console.WriteLine("Event1");
        }

        public void Apply(Event2 @event)
        {
            Console.WriteLine("Event2");
        }
    }

    public interface IEvent
    { }

    public class Event1 : IEvent { }

    public class Event2 : IEvent { }
}
