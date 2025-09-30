using System.Collections.Generic;
using System.Linq;

namespace OpenLibraryApp
{
    public class Book
    {
        public string Title { get; set; }
        public int? FirstPublishYear { get; set; }
        public List<string> Languages { get; set; }
        public double? AverageRating { get; set; }
        public string AuthorName { get; set; }

        public Book()
        {
            Languages = new List<string>();
        }

        public override string ToString() //ako pokusamo da ispisemo objekat klase Book, poziva se ova metoda, formatira u lep,citljiv tekst
        {
            var languagesStr = Languages.Any() ? string.Join(", ", Languages) : "Nije dostupno";
            var ratingStr = AverageRating.HasValue ? $"{AverageRating:F2}/5" : "Nije dostupno";
            var yearStr = FirstPublishYear.HasValue ? FirstPublishYear.ToString() : "Nije dostupno";
            // Dodavanje autora u izlazni string
            var authorStr = string.IsNullOrEmpty(AuthorName) ? "Nije dostupno" : AuthorName;

            return $"Naslov: {Title}\n" +
                   $"  - Autor: {authorStr}\n" +
                   $"  - Godina prvog izdanja: {yearStr}\n" +
                   $"  - Jezici: {languagesStr}\n" +
                   $"  - Prosečan rejting: {ratingStr}";
        }
    }
}