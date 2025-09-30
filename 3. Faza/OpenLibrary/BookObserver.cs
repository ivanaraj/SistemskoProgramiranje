using System;

namespace OpenLibraryApp
{
    public class BookObserver : IObserver<Book>
    {
        private readonly string _observerName;

        public BookObserver(string name)
        {
            _observerName = name;
        }

        public void OnNext(Book book)
        {
            // Log uspešno obrađenog zahteva (jedne knjige)
            Console.WriteLine($"\n--- {_observerName} | Novi podatak ---");
            Console.WriteLine(book.ToString());
        }

        public void OnError(Exception error)
        {
            // Log greške
            Console.WriteLine($"[ERROR] {_observerName}: Došlo je do greške u toku! Poruka: {error.Message}");
        }

        public void OnCompleted()
        {
            // Log uspešnog završetka
            Console.WriteLine($"--- {_observerName}: Pretraga je uspešno završena. ---");
        }
    }
}