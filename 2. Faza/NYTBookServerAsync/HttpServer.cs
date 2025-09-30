using System;
using System.Net;
using System.Threading.Tasks;

class HttpServer
{
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
                HttpListenerContext context = await listener.GetContextAsync(); //poboljsanje jer ne blokira celu petlju dok ne stigne zahtev,
                                                                                //ova metoda asinhrono ceka, petlja "spava" bez trosenja resursa
                                                                                //i "probudi" se tek kada stigne novi zahtev

                _ = Program.ProcessRequestAsync(context); // fire and forget poziv,
                                                          // kada stigne zahtev, odmah se pokrece njegova obrada
                                                          // nema await, ne ceka da se obrada zavrsi,
                                                          // vec se odmah vraca na pocetak petlje i ceka sledeci zahtev
                                                          // _ = svesno ignorise Task koji vraca metoda
                                                          // moze da primi na stotine zahteva istovremeno, svaki se obradi u pozadini
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GRESKA] Greska u petlji servera: {ex.Message}");
            }
        }
    }
}