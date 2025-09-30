using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenLibraryApp
{
    public class OpenLibraryService
    {
        private readonly HttpClient _client = new HttpClient(); //kao internet browser, koristimo ga jednom i to za sve zahteve

        private const string BaseUrl = "https://openlibrary.org/search.json";

        public async Task<IEnumerable<Book>> SearchWorksByAuthorAsync(string authorName)
        {
            var encodedAuthorName = Uri.EscapeDataString(authorName); // URL enkodiranje imena autora za ispravan API poziv
            var url = $"{BaseUrl}?author={encodedAuthorName}&limit=20"; // sastavljanje kompletnog URL-a za poziv i limitiranje na 20 rezultata

            try
            {
                Console.WriteLine($"[LOG] Salje se zahtev na: {url}");

                var response = await _client.GetAsync(url); //salje se get zahtev i ceka se odgovor
                response.EnsureSuccessStatusCode(); //provera da li je odgovor ok
                var content = await response.Content.ReadAsStringAsync(); //citanje sadrzaja kao string (bice JSON)

                var jsonResponse = JObject.Parse(content); //parsiranje tekstualnog JSON-a u objekat koji mozemo da pretrazujemo
                var docsJson = jsonResponse["docs"] as JArray; //iz odgovora izvlacimo niz dokumenata, gde je svaki dokument jedna knjiga

                if (docsJson == null)
                {
                    return Enumerable.Empty<Book>();
                }

                Console.WriteLine($"[LOG] Primljeno {docsJson.Count} rezultata za autora '{authorName}'.");

                // mapiranje: pretvaranje svakog JSON objekta u instancu klase Book
                return docsJson.Select(doc => new Book
                {
                    Title = (string)doc["title"],
                    FirstPublishYear = (int?)doc["first_publish_year"],
                    AverageRating = (double?)doc["ratings_average"],
                    Languages = (doc["language"] as JArray)?
                                .Select(l => (string)l)
                                .ToList() ?? new List<string>()
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Greska pri pretrazi za autora '{authorName}': {ex.Message}");
                return Enumerable.Empty<Book>();
            }
        }
    }
}