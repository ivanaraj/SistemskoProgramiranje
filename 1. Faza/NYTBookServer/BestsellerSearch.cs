// BestsellerSearch.cs
using System;
using System.Net;
using Newtonsoft.Json;

public class BestsellerSearch
{
    // !!! VAŽNO: Unesite vaš NYT API ključ ovde !!!
    private static readonly string ApiKey = "MI4GPkmyXlMiOqH6t8FaXZ74qXS0bWd9";

    public static string GetBestsellersByList(string listNameQuery)
    {
        Console.WriteLine($"[LOG] Obrada zahteva za listu: '{listNameQuery}'");

        // Provera keša uz zaključavanje
        lock (Cache.cacheLock)
        {
            if (Cache.cache.ContainsKey(listNameQuery))
            {
                Console.WriteLine($"[LOG] Odgovor pronadjen u kesu za listu: '{listNameQuery}'. Vracam kesiranu verziju.");
                return Cache.cache[listNameQuery];
            }
        }

        Console.WriteLine($"[LOG] Odgovor nije u kesu. Saljem zahtev ka NYT Books API-ju za listu: '{listNameQuery}'.");

        try
        {
            // Novi endpoint u skladu sa dokumentacijom
            string url = $"https://api.nytimes.com/svc/books/v3/lists/current/{Uri.EscapeDataString(listNameQuery)}.json?api-key={ApiKey}";

            HttpServer.client.Headers.Add("User-Agent", "C# App");
            string responseBody = HttpServer.client.DownloadString(url);

            var nytResponse = JsonConvert.DeserializeObject<NYTBestsellerResponse>(responseBody);

            if (nytResponse.Results == null || nytResponse.Results.Books == null || nytResponse.Results.Books.Count == 0)
            {
                Console.WriteLine($"[LOG] Nema rezultata za listu: '{listNameQuery}'.");
                return "<html><body><h1>Greška</h1><p>Nema knjiga za trazenu bestseler listu, ili ime liste nije ispravno.</p></body></html>";
            }

            // Kreiranje HTML odgovora sa podacima o bestselerima
            string result = "<html><head><title>Bestseleri: " + nytResponse.Results.DisplayName + "</title></head><body>";
            result += $"<h1>Prikaz bestselera sa liste: {nytResponse.Results.DisplayName}</h1>";
            foreach (var book in nytResponse.Results.Books)
            {
                result += $"<div>" +
                          $"<h3>#{book.Rank}. {book.Title}</h3>" +
                          $"<p><strong>Autor:</strong> {book.Author}</p>" +
                          $"<p><strong>Izdavac:</strong> {book.Publisher}</p>" +
                          $"<p><i>{book.Description}</i></p>" +
                          $"<a href='{book.AmazonProductUrl}' target='_blank'>Kupi na Amazonu</a>" +
                          $"<hr></div>";
            }
            result += "</body></html>";

            // Dodavanje u keš
            lock (Cache.cacheLock)
            {
                if (Cache.cacheIsEmpty == 0)
                {
                    Cache.cacheCleanupTimer.Start();
                    Cache.cacheIsEmpty = 1;
                    Console.WriteLine("[LOG] Kes je bio prazan. Startovan je tajmer za ciscenje kesa.");
                }

                Cache.cache.Add(listNameQuery, result);
                Console.WriteLine($"[LOG] Odgovor za listu '{listNameQuery}' je uspesno sacuvan u kesu.");
            }

            return result;
        }
        catch (WebException wex)
        {
            HttpWebResponse errorResponse = wex.Response as HttpWebResponse;
            if (errorResponse != null)
            {
                if (errorResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("[GRESKA] Doslo je do greske pri autorizaciji. Proverite da li je API kljuc ispravan.");
                    return "<html><body><h1>Greska servera</h1><p>Problem sa autorizacijom. Proverite API kljuc.</p></body></html>";
                }
                if (errorResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"[GRESKA] Lista '{listNameQuery}' nije pronadjena.");
                    return $"<html><body><h1>Greška 404</h1><p>Bestseler lista pod nazivom '{listNameQuery}' nije pronadjena. Proverite ime liste.</p></body></html>";
                }
            }

            Console.WriteLine($"[GRESKA] Web greska: {wex.Message}");
            return "<html><body><h1>Greska</h1><p>Doslo je do problema prilikom komunikacije sa NYT servisom.</p></body></html>";
        }
        catch (Exception e)
        {
            Console.WriteLine($"[GRESKA] Desila se nepredvidjena greska: {e.Message}");
            return "<html><body><h1>Greska</h1><p>Doslo je do nepredvidjene greske na serveru.</p></body></html>";
        }
    }
}