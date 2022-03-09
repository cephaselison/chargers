using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend
{
    public class Queue
    {
        public IList<Action<Charger, string>> _subscribers = new List<Action<Charger, string>>();

        public void Subscribe(Action<Charger, string> subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public void NotifyChange(Charger charger, string changedProperty)
        {
            Console.WriteLine($"{changedProperty} changed for charger {charger.Id}");

            foreach(var subscriber in _subscribers)
            {
                subscriber.Invoke(charger, changedProperty);
            }
        }
        
        private static readonly Lazy<Queue> _instance = new Lazy<Queue>();

        public static Queue GetInstance()
        {
            return _instance.Value;
        }
    }
}