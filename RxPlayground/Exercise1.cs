using System;
using System.Reactive.Linq;

namespace RxPlayground
{
    /// <summary>
    /// This example demonstrates the elementary concepts of creating an observable from an event pattern
    /// in a simple console application. While in a UI application it is very easy to hook into any of the
    /// hundreds of an event to react to, in a simple application, the options are pretty less. In this example,
    /// we are going to create a class that acts as an event source.
    /// </summary>
    public class Exercise1
    {
        public static void Run()
        {
            var obj = new MyClass();
            var myObservable =
                Observable.FromEventPattern<ValueChangedEventHandler, ValueChangedEventArgs>(
                    h => obj.OnStateChanged += h,
                    h => obj.OnStateChanged -= h
                );

            myObservable
                .Where(e => e.EventArgs.NewValue % 2 == 0)
                .Select(ep => ep.EventArgs)
                .Subscribe(e =>
                {
                    Console.WriteLine($"New value is {e.NewValue}");
                });

            while (true)
            {
                if (Int32.TryParse(Console.ReadLine(), out var temp))
                    obj.State = temp;
                else
                    break;
            }
        }
        
        class MyClass
        {
            public event ValueChangedEventHandler OnStateChanged;
            public int State
            {
                get => _state;
                set
                {
                    var old = _state;
                    _state = value;
                    OnStateChanged?.Invoke(this, new ValueChangedEventArgs { NewValue = _state});
                }
            }

            int _state;
        
        }
    }
    class ValueChangedEventArgs : EventArgs
    {
        public int NewValue { get; set; }
    }
    
    internal delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs args);
}