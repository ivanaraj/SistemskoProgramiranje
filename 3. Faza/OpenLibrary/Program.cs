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
        Console.WriteLine("Unesite ime autora (ili vise autora odvojenih zarezom, npr. 'J R R Tolkien, Isaac Asimov'):");
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

        Console.WriteLine($"\nPokrecem pretragu za: {string.Join(", ", authors)}\n");

        // kreiranje liste reaktivnih tokova, po jedan za svakog autora
        var streams = authors.Select(author => new AuthorWorksStream(author)).ToList();

        // spajanje svih tokova u jedan jedinstveni tok pomocu Observable.Merge
        var combinedStream = Observable.Merge(streams.Cast<IObservable<Book>>());

        // kreiranje posmatraca
        var bookObserver = new BookObserver("Glavni posmatrac");

        // pretplata posmatraca na spojeni tok
        var subscription = combinedStream.Subscribe(bookObserver);

        // pokretanje asinhronog pribavljanja podataka za sve tokove paralelno
        var fetchTasks = streams.Select(s => s.GetWorksAsync()).ToList();

        // cekanje da se svi zadaci zavrse
        await Task.WhenAll(fetchTasks);

        Console.WriteLine("\nSvi podaci su preuzeti. Pritisnite ENTER za izlaz...");
        Console.ReadLine();

        // oslobadjanje resursa
        subscription.Dispose();
    }
}