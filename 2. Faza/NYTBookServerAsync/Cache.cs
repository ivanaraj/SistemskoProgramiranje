// Cache.cs
using System;
using System.Collections.Generic;

class Cache
{
    public static int cacheIsEmpty = 0;
    public static readonly Dictionary<string, string> cache = new Dictionary<string, string>();

    // Objekat za zaključavanje radi bezbednog pristupa kešu iz više niti
    public static readonly object cacheLock = new object();

    public static readonly System.Timers.Timer cacheCleanupTimer = new System.Timers.Timer(TimeSpan.FromMinutes(10).TotalMilliseconds);

    public static void CacheCleanup()
    {
        Console.WriteLine("[LOG] Tajmer je istekao. Brisem kompletan kes.");
        // Zaključavamo pristup da ne bi došlo do konflikta sa drugim nitima
        lock (cacheLock)
        {
            Cache.cache.Clear();
            cacheIsEmpty = 0;
        }
        cacheCleanupTimer.Stop();
        Console.WriteLine("[LOG] Kes je obrisan. Tajmer je zaustavljen.");
    }
}