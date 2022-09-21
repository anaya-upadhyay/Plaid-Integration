namespace IntegratePlaid.Controllers
{
    internal class PlaidParams
    {
        public string client_id { get; set; }
        public string secret { get; set; }
        public string client_name { get; set; }
        public List<string> country_codes { get; set; }
        public string language { get; set; }
        public PlaidUser user { get; set; }
        public List<string> products { get; set; }
        /// <summary>
        /// Add the following property for Micro-Deposit.
        /// This is supported only on ["auth"] product.
        /// </summary>
        public LinkAuth auth { get; set; }
    }

    internal class PlaidParamsVerification : PlaidParams
    {
        public string access_token { get; set; }
    }

    internal class AccessTokenParams
    {
        public string client_id { get; set; }
        public string secret { get; set; }
        public string public_token { get; set; }
    }

    internal class PlaidUser
    {
        public string client_user_id { get; set; }
    }

    /// <code>
    /// {
    ///   "expiration": "2022-08-30T23:35:28Z",
    ///   "link_token": "link-sandbox-5c97b355-a9da-4dec-98a1-3b0d0093c01d",
    ///   "request_id": "Bg6iD3kKeMfbzmP"
    /// }
    /// </code>
    internal class LinkResponse
    {
        public string expiration { get; set; }
        public string link_token { get; set; }
        public string request_id { get; set; }
    }

    internal class AccessTokenResponse
    {
        public string access_token { get; set; }
        public string item_id { get; set; }
        public string request_id { get; set; }
    }

    /// <summary>
    /// For Micro-Deposit
    /// </summary>
    internal class LinkAuth
    {
        public bool same_day_microdeposits_enabled { get; set; }
        public bool auth_type_select_enabled { get; set; }
    }

    internal class ProcessTokenParams
    {
        public string client_id { get; set; }
        public string secret { get; set; }
        public string access_token { get; set; }
        public string account_id { get; set; }
        public string processor { get; set; }
    }

    internal class ProcessorTokenResponse
    {
        public string processor_token { get; set; }
        public string request_id { get; set; }
    }

    internal class ACHRelationshipParams
    {
        public string account_owner_name { get; set; }
        public string bank_account_type { get; set; }
        public string bank_account_number { get; set; }
        public string bank_routing_number { get; set; }
        public string nickname { get; set; }
        public string processor_token { get; set; }
    }

    internal class ACHRelationshipResponse
    {
        public string id { get; set; }
        public string account_id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string status { get; set; }
        public string account_owner_name { get; set; }
        public string nickname { get; set; }
    }
}
