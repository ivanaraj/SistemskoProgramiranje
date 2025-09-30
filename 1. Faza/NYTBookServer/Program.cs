using System;
using System.IO;
using System.Net;
using System.Text;

class Program
{
    static void Main()
    {
        // event subscription
        Cache.cacheCleanupTimer.Elapsed += (sender, e) => Cache.CacheCleanup();
        HttpServer.StartServer();
    }

    public static void ProcessRequest(object state)
    {
        HttpListenerContext context = (HttpListenerContext)state;

        try
        {
            string path = context.Request.Url.AbsolutePath.Trim('/'); // uzima putanju bez pocetnog i zavrsnog '/'

            if (path.Equals("favicon.ico", StringComparison.OrdinalIgnoreCase)) // ignorise zahtev
            {
                context.Response.StatusCode = 204; // sve je u redu, nema sadrzaja
                context.Response.Close();
                return; // izlazi odmah, nista se ne loguje
            }

            Console.WriteLine($"[LOG] Primljen zahtev: {context.Request.Url}");

            string query = context.Request.RawUrl.Substring(1); // uzima putanju bez pocetnog '/'
            query = Uri.UnescapeDataString(query); // dekodira URL-encoded znakove

            if (string.IsNullOrWhiteSpace(query)) // ukoliko se prosledi prazan zahtev
            {
                byte[] buffer = Encoding.UTF8.GetBytes
                (
                    "<html><body>Molimo unesite ime bestseler liste u URL (npr. http://localhost:5000/hardcover-fiction).</body></html>"
                );

                context.Response.ContentLength64 = buffer.Length; //browser mora da zna koliko podataka da ocekuje
                context.Response.ContentType = "text/html"; // da browse zna kako da prikaze sadrzaj
                Stream output = context.Response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();

                Console.WriteLine("[LOG] Odgovoreno na prazan zahtev.");
                return;
            }

            // poziv logike za pretragu bestseller lista
            string responseString = BestsellerSearch.GetBestsellersByList(query);

            byte[] buffer2 = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer2.Length;
            context.Response.ContentType = "text/html";
            Stream output2 = context.Response.OutputStream;
            output2.Write(buffer2, 0, buffer2.Length);
            output2.Close();

            Console.WriteLine($"[LOG] Zahtev za listu '{query}' je uspešno obradjen i odgovor je poslat.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FATALNA GRESKA] Desila se greska pri obradi zahteva: {ex.Message}");
            if (context.Response.OutputStream.CanWrite)
            {
                context.Response.OutputStream.Close();
            }
        }
    }
}