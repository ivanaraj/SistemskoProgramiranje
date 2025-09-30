using System;
using System.Net;
using System.Threading;

class HttpServer
{
    public static WebClient client = new WebClient(); //za slanje zahteva drugim serverima i preuzimanje podataka sa interneta,
                                                      //koristi se samo jedna instanca koju ce deliti cela aplikacija,
                                                      //za kontaktiranje NYT Books API-ja

    public static void StartServer()
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:5000/");
        listener.Start();
        Console.WriteLine("Server je pokrenut na adresi http://localhost:5000/");
        Console.WriteLine("Cekam na zahteve...");

        while (true)
        {
            try
            {
                HttpListenerContext context = listener.GetContext(); // blokira dok ne stigne novi zahtev od browser-a,
                                                                     // kada stigne, podaci se smestaju u context i program nastavlja dalje
                ThreadPool.QueueUserWorkItem(new WaitCallback(Program.ProcessRequest), context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GRESKA] Greska u petlji servera: {ex.Message}");
            }
        }
    }
}