// Program.cs
using System;
using System.IO;
using System.Net;
using System.Text;

class Program
{
    static void Main()
    {
        Cache.cacheCleanupTimer.Elapsed += (sender, e) => Cache.CacheCleanup();
        HttpServer.StartServer();
    }

    //public static void ProcessRequest(object state)
    //{
    //    HttpListenerContext context = (HttpListenerContext)state;

    //    try
    //    {
    //        Console.WriteLine($"[LOG] Primljen zahtev: {context.Request.Url}");

    //        string query = context.Request.RawUrl.Substring(1);
    //        query = Uri.UnescapeDataString(query);


    //        if (string.IsNullOrWhiteSpace(query))
    //        {
    //            // Ažurirana poruka da uputi korisnika na novu funkcionalnost
    //            byte[] buffer = Encoding.UTF8.GetBytes("<html><body>Molimo unesite ime bestseler liste u URL (npr. http://localhost:5000/hardcover-fiction).</body></html>");
    //            context.Response.ContentLength64 = buffer.Length;
    //            context.Response.ContentType = "text/html";
    //            Stream output = context.Response.OutputStream;
    //            output.Write(buffer, 0, buffer.Length);
    //            output.Close();
    //            Console.WriteLine("[LOG] Odgovoreno na prazan zahtev.");
    //            return;
    //        }

    //        // Pozivamo novu logiku za pretragu bestseler lista
    //        string responseString = BestsellerSearch.GetBestsellersByList(query);

    //        byte[] buffer2 = Encoding.UTF8.GetBytes(responseString);
    //        context.Response.ContentLength64 = buffer2.Length;
    //        context.Response.ContentType = "text/html";
    //        Stream output2 = context.Response.OutputStream;
    //        output2.Write(buffer2, 0, buffer2.Length);
    //        output2.Close();
    //        Console.WriteLine($"[LOG] Zahtev za listu '{query}' je uspešno obrađen i odgovor je poslat.");
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"[FATALNA GREŠKA] Desila se greška pri obradi zahteva: {ex.Message}");
    //        if (context.Response.OutputStream.CanWrite)
    //        {
    //            context.Response.OutputStream.Close();
    //        }
    //    }
    //}



    public static void ProcessRequest(object state)
    {
        HttpListenerContext context = (HttpListenerContext)state;

        try
        {
            string path = context.Request.Url.AbsolutePath.Trim('/');

            // favicon.ico -> potpuno ignorisati
            if (path.Equals("favicon.ico", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = 204; // No Content
                context.Response.Close();
                return; // izlazi odmah, ništa se ne loguje
            }

            // Sada je sigurno da nije favicon, loguj ostale zahteve
            Console.WriteLine($"[LOG] Primljen zahtev: {context.Request.Url}");

            string query = context.Request.RawUrl.Substring(1);
            query = Uri.UnescapeDataString(query);

            if (string.IsNullOrWhiteSpace(query))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(
                    "<html><body>Molimo unesite ime bestseler liste u URL (npr. http://localhost:5000/hardcover-fiction).</body></html>"
                );
                context.Response.ContentLength64 = buffer.Length;
                context.Response.ContentType = "text/html";
                Stream output = context.Response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();

                Console.WriteLine("[LOG] Odgovoreno na prazan zahtev.");
                return;
            }

            // Poziv logike za pretragu bestseler lista
            string responseString = BestsellerSearch.GetBestsellersByList(query);

            byte[] buffer2 = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer2.Length;
            context.Response.ContentType = "text/html";
            Stream output2 = context.Response.OutputStream;
            output2.Write(buffer2, 0, buffer2.Length);
            output2.Close();

            Console.WriteLine($"[LOG] Zahtev za listu '{query}' je uspešno obrađen i odgovor je poslat.");
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