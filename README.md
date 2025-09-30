1. Faza
   
Napomene za izradu domaćeg zadatka:
Web server implementirati kao konzolnu aplikaciju koja loguje sve primljene zahteve i informacije o
njihovoj obradi (da li je došlo do greške, da li je zahtev uspešno obrađen i ostale ključe detalje).
Web server treba da kešira u memoriji odgovore na primljene zahteve, tako da u slučaju da stigne
isti zahtev, prosleđuje se već pripremljeni odgovor. Kao klijentsku aplikaciju možete koristiti Web
browser ili možete po potrebi kreirati zasebnu konzolnu aplikaciju. Za realizaciju koristiti funkcije
iz biblioteke System.Threading, uključujući dostupne mehanizme za sinhronizaciju i zaključavanje.
Dozvoljeno je korišćenje ThreadPool-a.
Napomene za predaju domaćeg zadatka:
Prvi projekta pokriva temu višenitnog programiranja korišćenjem .NET Framework-a. Za izradu
projekta treba korstiti C# programski jezik.

Zadatak 17:
Kreirati Web server koji klijentu omogućava pretagu knjiga korišćenjem New York Times APIa. 
Pretraga se može vršiti pomoću filtera koji se definišu u okviru query-a. Spisak knjiga koje
zadovoljavaju uslov se vraćaju kao odgovor klijentu (pretragu knjiga vršiti po autoru). Svi zahtevi
serveru se šalju preko browser-a korišćenjem GET metode. Ukoliko navedene knjige ne postoje,
prikazati grešku klijentu.
Način funkcionisanja New York Times API-a je moguće proučiti na sledećem linku:
https://developer.nytimes.com/docs/books-product/1/overview
Primer poziva serveru:
https://api.nytimes.com/svc/books/v3/reviews.json?author=Stephen+King&api-key=yourkey

(zbog promene funkcionalnosti ovog api kljuca, prilagodjeno je resenje zadatka koje se razlikuje u odnosu na zadati tekst)


2. Faza
   
Za drugi projekat timovi treba da implementiraju isti zadatak koji su imali za prvi projekat,
uz izmenu da sada treba koristiti taskove i asinhrone operacije (tamo gde to ima smisla).
Za obradu kod koje taskovi nemaju smisla treba zadržati klasične niti.
Dozvoljeno je korišćenje mehanizama za međusobno zaključavanje i sinhronizaciju.

3. Faza

Napomene za izradu domaćeg zadatka:
Web server implementirati kao konzolnu aplikaciju koja loguje sve primljene zahteve i informacije
o njihovoj obradi (da li je došlo do greške, da li je zahtev uspešno obrađen i ostale ključe detalje).
Kao klijentsku aplikaciju možete koristiti Web browser ili možete po potrebi kreirati zasebnu
konzolnu aplikaciju.
Za realizaciju koristiti biblioteku Reactive Extensions for .NET (Rx) i implementirati
odgovarajuće paradigme Reaktivnog programiranja. Po defaultu, Rx je single-threaded rešenje.
Implementacije koje uključuju rad sa multithreadingom i radom sa Schedulerima će biti ocenjenje
većim brojem poena. Korišćenje Rx biblioteke je neophodno, dok po potrebi, možete uključiti i
potrebne biblioteke zavisno od zahteva navedenih u zadacima. Biblioteke su date kao preporuka,
dozvoljena je upotreba i drugih rešenja (na primer za Sentiment analizu, Topic Modeling).

Zadatak 21:
Koristeći principe Reaktivnog programiranja i OpenLibrary API-a, implementirati aplikaciju za
prikaz informacija o autorima i njihovim knjigama. Koristiti opciju za pretragu po odgovarajućem
autoru. Potrebno je voditi evidenciju o nazivima knjiga, godini izdavanja, jezicima na koje je
prevedena, kao i prosečnom rejtingu. Prikazati dobijene rezultate.
Dokumentacija dostupna na linku: https://openlibrary.org/dev/docs/api/search
