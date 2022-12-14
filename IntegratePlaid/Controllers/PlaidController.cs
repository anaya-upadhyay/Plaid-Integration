using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using RestSharp;

namespace IntegratePlaid.Controllers
{
    public class PlaidController : Controller
    {
        private readonly ILogger<PlaidController> _logger;
        private readonly PlaidSettingsConfiguration _plaidSettings;

        private const string PLAID_SANDBOX = "sandbox";
        private const string PLAID_DEVELEOPMENT = "development";

        public PlaidController(ILogger<PlaidController> logger,
                               IOptions<PlaidSettingsConfiguration> options)
        {
            _logger = logger;
            _plaidSettings = options.Value;
        }

        public IActionResult Index()
        {
            var client = new RestClient(_plaidSettings.Environment is PLAID_SANDBOX ? _plaidSettings.LinkTokenURL : _plaidSettings.DevTokenURL);
                        
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/json");

            var param = new PlaidParams
            {
                client_id = _plaidSettings.Environment is PLAID_SANDBOX ? _plaidSettings.ClientId: _plaidSettings.DevClientId,
                secret = _plaidSettings.Environment is PLAID_SANDBOX ? _plaidSettings.ClientSecret: _plaidSettings.DevSecret,
                client_name = "Wealthlane",
                country_codes = new List<string> { "US" },
                language = "en",
                user = new PlaidUser { client_user_id = "Anaya Upadhyay" },
                products = new List<string> { "auth" },

                // Un-comment the following line if you want to use Micro-Deposit
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

            var request = new RestRequest();
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
             
                var link_token = GenerateLinkTokenForVerification(ViewBag.AccessToken);

                return Json(new { AccessToken = ViewBag.AccessToken, LinkToken = link_token });
            }
            else return BadRequest($"{response.StatusCode}: {response.Content}");
        }

        public string GenerateLinkTokenForVerification(string access_token)
        {
            var client = new RestClient(_plaidSettings.Environment is PLAID_SANDBOX ? _plaidSettings.LinkTokenURL : _plaidSettings.DevTokenURL);

            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/json");

            var param = new PlaidParamsVerification
            {
                client_id = _plaidSettings.Environment is PLAID_SANDBOX ? _plaidSettings.ClientId : _plaidSettings.DevClientId,
                secret = _plaidSettings.Environment is PLAID_SANDBOX ? _plaidSettings.ClientSecret : _plaidSettings.DevSecret,
                client_name = "Wealthlane",
                country_codes = new List<string> { "US" },
                language = "en",
                user = new PlaidUser { client_user_id = "Anaya Upadhyay" },
                access_token = access_token,
            };

            request.AddJsonBody(param);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<LinkResponse>(response.Content);
                ViewBag.VerifyToken = result.link_token;

                return result.link_token;
            }

            ViewBag.Environment = _plaidSettings.Environment;
            return "";
        }

    }
}