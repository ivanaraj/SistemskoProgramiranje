using System;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace OpenLibraryApp
{
    public class AuthorWorksStream : IObservable<Book>
    {
        private readonly Subject<Book> _bookSubject = new Subject<Book>(); //poseban objekat koji je istovremeno i posmatrac
                                                                           //i posmatran(drugi mogu da se pretplate na njega)
        private readonly OpenLibraryService _libraryService = new OpenLibraryService();
        public string AuthorName { get; }

        public AuthorWorksStream(string authorName)
        {
            AuthorName = authorName;
        }

        // pribavljanje i emitovanje podataka(knjiga)
        public async Task GetWorksAsync()
        {
            try
            {
                var books = await _libraryService.SearchWorksByAuthorAsync(AuthorName); //pozivanje servisa da pronadje sve knjige za datog autora, ceka dok se ne zavrsi

                if (!books.Any())
                {
                    Console.WriteLine($"[LOG] Nema pronadjenih dela za autora: {AuthorName}");
                }

                foreach (var book in books)
                {
                    book.AuthorName = AuthorName;
                    _bookSubject.OnNext(book);  //guramo knjigu u nas tok, svi koji su se pretplatili na nas tok ce istog trenutka dobiti ovu knjigu
                }
                _bookSubject.OnCompleted(); //kada su se poslale sve knjige, saljemo obavestenje da je tok zavrsen
            }
            catch (Exception ex)
            {
                _bookSubject.OnError(ex);
            }
        }

        public IDisposable Subscribe(IObserver<Book> observer)
        {
            return _bookSubject.Subscribe(observer); //povezuje posmatraca sa tokom
        }
    }
}