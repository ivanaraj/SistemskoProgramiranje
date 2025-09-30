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
        private readonly HttpClient _client = new HttpClient();
        private const string BaseUrl = "https://openlibrary.org/search.json";

        public async Task<IEnumerable<Book>> SearchWorksByAuthorAsync(string authorName)
        {
            // URL enkodiranje imena autora za ispravan API poziv
            var encodedAuthorName = Uri.EscapeDataString(authorName);
            var url = $"{BaseUrl}?author={encodedAuthorName}&limit=20"; // Limitiramo na 20 rezultata

            try
            {
                Console.WriteLine($"[LOG] Šalje se zahtev na: {url}");
                var response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                var jsonResponse = JObject.Parse(content);
                var docsJson = jsonResponse["docs"] as JArray;

                if (docsJson == null)
                {
                    return Enumerable.Empty<Book>();
                }

                Console.WriteLine($"[LOG] Primljeno {docsJson.Count} rezultata za autora '{authorName}'.");

                // Mapiranje JSON-a u listu Book objekata
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
                Console.WriteLine($"[ERROR] Greška pri pretrazi za autora '{authorName}': {ex.Message}");
                return Enumerable.Empty<Book>();
            }
        }
    }
}