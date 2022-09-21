namespace IntegratePlaid
{
    public class PlaidSettingsConfiguration
    {
        public string Environment { get; set; }

        public string LinkTokenURL { get; set; }
        public string PublicTokenURL { get; set; }
        public string AccessTokenURL { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string DevTokenURL { get; set; }
        public string DevClientId { get; set; }
        public string DevSecret { get; set; }

        public string ProcessorTokenURL { get; set; }
    }

}
