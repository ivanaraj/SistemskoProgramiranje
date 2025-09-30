using OpenLibraryApp;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("--- Aplikacija za pretragu knjiga po autoru ---");
        Console.WriteLine("Unesite ime autora (ili više autora odvojenih zarezom, npr. 'J R R Tolkien, Isaac Asimov'):");
        string input = Console.ReadLine();

        var authors = input?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                           .Select(a => a.Trim())
                           .Where(a => !string.IsNullOrEmpty(a))
                           .ToList();

        if (authors == null || !authors.Any())
        {
            Console.WriteLine("Niste uneli nijedno ime autora. Izlazak.");
            return;
        }

        Console.WriteLine($"\nPokrećem pretragu za: {string.Join(", ", authors)}\n");

        // Kreiranje liste reaktivnih tokova, po jedan za svakog autora
        var streams = authors.Select(author => new AuthorWorksStream(author)).ToList();

        // Spajanje svih tokova u jedan jedinstveni tok pomoću Observable.Merge
        var combinedStream = Observable.Merge(streams.Cast<IObservable<Book>>());

        // Kreiranje posmatrača
        var bookObserver = new BookObserver("Glavni posmatrač");

        // Pretplata posmatrača na spojeni tok
        var subscription = combinedStream.Subscribe(bookObserver);

        // Pokretanje asinhronog pribavljanja podataka za sve tokove paralelno
        var fetchTasks = streams.Select(s => s.GetWorksAsync()).ToList();

        // Čekanje da se svi zadaci završe
        await Task.WhenAll(fetchTasks);

        Console.WriteLine("\nSvi podaci su preuzeti. Pritisnite ENTER za izlaz...");
        Console.ReadLine();

        // Oslobađanje resursa
        subscription.Dispose();
    }
}