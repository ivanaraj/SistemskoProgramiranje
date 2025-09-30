// BestsellerList.cs
using System.Collections.Generic;
using Newtonsoft.Json;

public class BestsellerList
{
    [JsonProperty("list_name_encoded")]
    public string ListNameEncoded { get; set; }

    [JsonProperty("display_name")]
    public string DisplayName { get; set; }

    [JsonProperty("books")]
    public List<BestsellerBook> Books { get; set; }
}