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

        public void OnNext(Book book) //poziva se svaki put kada stigne nova knjiga 
        {
            Console.WriteLine($"\n--- {_observerName} | Novi podatak ---");
            Console.WriteLine(book.ToString());
        }

        public void OnError(Exception error) //poziva se ako dodje do greske u toku
        {
            // Log greške
            Console.WriteLine($"[ERROR] {_observerName}: Doslo je do greske u toku! Poruka: {error.Message}");
        }

        public void OnCompleted() //poziva se kada je tok zavrsen
        {
            Console.WriteLine($"--- {_observerName}: Pretraga je uspesno zavrsena. ---");
        }
    }
}