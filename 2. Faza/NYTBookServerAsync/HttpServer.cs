// HttpServer.cs
using System;
using System.Net;
using System.Threading.Tasks;

class HttpServer
{
    // WebClient je zadržan radi jednostavnosti, mada se u modernim aplikacijama
    // preporučuje korišćenje HttpClient klase.
    public static WebClient client = new WebClient();

    public static async Task StartServerAsync()
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
                // Asinhrono čekanje na dolazni zahtev
                HttpListenerContext context = await listener.GetContextAsync();

                // Pokrećemo obradu zahteva kao novi zadatak (Task)
                // i ne čekamo ga (fire and forget).
                // Ovo omogućava serveru da odmah primi sledeći zahtev.
                _ = Program.ProcessRequestAsync(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GRESKA] Greska u petlji servera: {ex.Message}");
            }
        }
    }
}