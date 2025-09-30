using Newtonsoft.Json;

public class NYTBestsellerResponse
{
    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("num_results")]
    public int NumberOfResults { get; set; }

    [JsonProperty("results")]
    public BestsellerList Results { get; set; }
}