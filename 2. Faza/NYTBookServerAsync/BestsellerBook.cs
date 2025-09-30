using Newtonsoft.Json;

public class BestsellerBook
{
    [JsonProperty("rank")]
    public int Rank { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("author")]
    public string Author { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("publisher")]
    public string Publisher { get; set; }

    [JsonProperty("amazon_product_url")]
    public string AmazonProductUrl { get; set; }
}