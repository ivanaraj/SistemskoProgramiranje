// Program.cs
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // Tajmer za čišćenje keša ostaje isti, jer radi u pozadini
        Cache.cacheCleanupTimer.Elapsed += (sender, e) => Cache.CacheCleanup();

        // Pokrećemo i čekamo asinhroni server
        await HttpServer.StartServerAsync();
    }

    public static async Task ProcessRequestAsync(HttpListenerContext context)
    {
        try
        {
            string path = context.Request.Url.AbsolutePath.Trim('/');

            if (path.Equals("favicon.ico", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                context.Response.Close();
                return;
            }

            Console.WriteLine($"[LOG] Primljen zahtev: {context.Request.Url}");

            string query = context.Request.RawUrl.Substring(1);
            query = Uri.UnescapeDataString(query);

            string responseString;

            if (string.IsNullOrWhiteSpace(query))
            {
                responseString = "<html><body>Molimo unesite ime bestseler liste u URL (npr. http://localhost:5000/hardcover-fiction).</body></html>";
                Console.WriteLine("[LOG] Odgovoreno na prazan zahtev.");
            }
            else
            {
                // Asinhrono pozivamo logiku za pretragu bestselera
                responseString = await BestsellerSearch.GetBestsellersByListAsync(query);
                Console.WriteLine($"[LOG] Zahtev za listu '{query}' je uspešno obrađen i odgovor je poslat.");
            }

            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.ContentType = "text/html";
            Stream output = context.Response.OutputStream;

            // Asinhrono slanje odgovora klijentu
            await output.WriteAsync(buffer, 0, buffer.Length);
            output.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FATALNA GREŠKA] Desila se greška pri obradi zahteva: {ex.Message}");
            if (context.Response.OutputStream.CanWrite)
            {
                context.Response.OutputStream.Close();
            }
        }
    }
}