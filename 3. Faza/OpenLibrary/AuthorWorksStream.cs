// AuthorWorksStream.cs
using System;
using System.Linq; // <--- Dodat using za .Any()
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace OpenLibraryApp
{
    // Stream knjiga jednog autora - IObservable<Book>
    public class AuthorWorksStream : IObservable<Book>
    {
        private readonly Subject<Book> _bookSubject = new Subject<Book>();
        private readonly OpenLibraryService _libraryService = new OpenLibraryService();
        public string AuthorName { get; }

        public AuthorWorksStream(string authorName)
        {
            AuthorName = authorName;
        }

        // Metoda za asinhrono pribavljanje i emitovanje podataka
        public async Task GetWorksAsync()
        {
            try
            {
                var books = await _libraryService.SearchWorksByAuthorAsync(AuthorName);
                if (!books.Any())
                {
                    Console.WriteLine($"[LOG] Nema pronađenih dela za autora: {AuthorName}");
                }

                foreach (var book in books)
                {
                    book.AuthorName = AuthorName; // <--- NOVO: Dodavanje imena autora
                    _bookSubject.OnNext(book); // Emitovanje svake knjige
                }
                _bookSubject.OnCompleted(); // Signaliziranje kraja
            }
            catch (Exception ex)
            {
                _bookSubject.OnError(ex);
            }
        }

        public IDisposable Subscribe(IObserver<Book> observer)
        {
            return _bookSubject.Subscribe(observer);
        }
    }
}