using System;
using System.Collections.Generic;
using ST10038937_prog7311_poe1.Models;

namespace ST10038937_prog7311_poe1.Services
{
    // Observer interface
    public interface IProductObserver
    {
        void OnProductCreated(Product product);
    }

    // Notifier class
    public class ProductNotifier
    {
        private readonly List<IProductObserver> _observers = new List<IProductObserver>();

        public void RegisterObserver(IProductObserver observer)
        {
            _observers.Add(observer);
        }

        public void NotifyProductCreated(Product product)
        {
            foreach (var observer in _observers)
            {
                observer.OnProductCreated(product);
            }
        }
    }

    // Example observer: logs product creation
    public class ProductLogObserver : IProductObserver
    {
        public void OnProductCreated(Product product)
        {
            // In a real app, use a logger; here, just simulate
            Console.WriteLine($"Product created: {product.Name} (ID: {product.ProductId})");
        }
    }
}
