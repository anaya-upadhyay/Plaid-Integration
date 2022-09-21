using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using RestSharp;
using RestSharp.Authenticators;

namespace IntegratePlaid.Controllers
{
    public class ACHController : Controller
    {
        private readonly PlaidSettingsConfiguration _plaidSettings;
        private readonly AlpacaConfiguration _alpacaSettings;

        private const string PLAID_SANDBOX = "sandbox";
        private const string PLAID_DEVELEOPMENT = "development";

        public ACHController(IOptions<PlaidSettingsConfiguration> options,
                             IOptions<AlpacaConfiguration> alpacaOptions)
        {
            _plaidSettings = options.Value;
            _alpacaSettings = alpacaOptions.Value;
        }

        public IActionResult Index()
        {
            var client = new RestClient(_plaidSettings.Environment is PLAID_SANDBOX ? _plaidSettings.LinkTokenURL : _plaidSettings.DevTokenURL);

            RestRequest request = new();
            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/json");

            var param = new PlaidParams
            {
                client_id = _plaidSettings.Environment is PLAID_SANDBOX ? _plaidSettings.ClientId : _plaidSettings.DevClientId,
                secret = _plaidSettings.Environment is PLAID_SANDBOX ? _plaidSettings.ClientSecret : _plaidSettings.DevSecret,
                client_name = "Wealthlane",
                country_codes = new List<string> { "US" },
                language = "en",
                user = new PlaidUser { client_user_id = "Anaya Upadhyay" },
                products = new List<string> { "auth" },

                auth = new LinkAuth { same_day_microdeposits_enabled = true, auth_type_select_enabled = true }
            };

            request.AddJsonBody(param);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<LinkResponse>(response.Content);
                ViewBag.LinkToken = result.link_token;
            }

            ViewBag.Environment = _plaidSettings.Environment;

            return View();
        }

        public IActionResult GetAccessToken(string publicToken)
        {
            var client = new RestClient(_plaidSettings.AccessTokenURL);

            RestRequest request = new();
            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/json");

            var param = new AccessTokenParams
            {
                client_id = _plaidSettings.Environment is PLAID_SANDBOX ? _plaidSettings.ClientId : _plaidSettings.DevClientId,
                secret = _plaidSettings.Environment is PLAID_SANDBOX ? _plaidSettings.ClientSecret : _plaidSettings.DevSecret,
                public_token = publicToken
            };

            request.AddJsonBody(param);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<AccessTokenResponse>(response.Content);

                ViewBag.AccessToken = result.access_token;
                ViewBag.NewEnvironment = _plaidSettings.Environment;

                return Json(new { AccessToken = ViewBag.AccessToken});
            }
            else return BadRequest($"{response.StatusCode}: {response.Content}");
        }

        public IActionResult CreateProcessorToken(string accessToken, string accountId)
        {
            var client = new RestClient(_plaidSettings.ProcessorTokenURL);

            RestRequest request = new();
            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/json");

            var param = new ProcessTokenParams
            {
                client_id = _plaidSettings.Environment is PLAID_SANDBOX ? _plaidSettings.ClientId : _plaidSettings.DevClientId,
                secret = _plaidSettings.Environment is PLAID_SANDBOX ? _plaidSettings.ClientSecret : _plaidSettings.DevSecret,
                access_token = accessToken,
                account_id = accountId,
                processor = "alpaca"
            };

            request.AddJsonBody(param);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<ProcessorTokenResponse>(response.Content);

                return Ok(result);
            }

            return Ok();
        }

        public IActionResult CreateAlpacaACHRelationship(string processor_token, string account_id)
        {
            var client = new RestClient($"{_alpacaSettings.BrokerAPISandbox}{_alpacaSettings.ACHRelationshipURL.Replace("{account_id}", account_id)}");

            RestRequest request = new();
            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/json");
            client.Authenticator = new HttpBasicAuthenticator(_alpacaSettings.BrokerAPIKey, _alpacaSettings.BrokerAPISecret);

            var param = new ACHRelationshipParams
            {
                account_owner_name = "Anaya Upadhyay",
                bank_account_type = "Plaid Savings",
                bank_account_number = "1111222233331111",
                bank_routing_number = "021000021",
                nickname = "Houndstooth Bank",
                processor_token = processor_token
            };

            request.AddJsonBody(param);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<ACHRelationshipResponse>(response.Content);

                return Ok(result);
            }

            return Ok();
        }
    }
}
