namespace NoviExchange.EcbClient.Options
{
    public class EcbClientOptions
    {
        public string EcbUrl { get; set; } = string.Empty;
        public int MaxRetries { get; set; }
        public int InitialDelayMs { get; set; }
    }
}
