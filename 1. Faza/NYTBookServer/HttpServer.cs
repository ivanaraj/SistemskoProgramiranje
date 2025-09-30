// HttpServer.cs
using System;
using System.Net;
using System.Threading;

class HttpServer
{
    public static WebClient client = new WebClient();

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
                HttpListenerContext context = listener.GetContext();
                // Svaki novi zahtev se obrađuje na posebnoj niti iz ThreadPool-a
                ThreadPool.QueueUserWorkItem(new WaitCallback(Program.ProcessRequest), context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GRESKA] Greska u petlji servera: {ex.Message}");
            }
        }
    }
}