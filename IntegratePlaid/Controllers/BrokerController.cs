using IntegratePlaid.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using RestSharp;
using RestSharp.Authenticators;

namespace IntegratePlaid.Controllers
{
    public class BrokerController : Controller
    {
        private readonly AlpacaConfiguration _config;

        public BrokerController(IOptions<AlpacaConfiguration> options)
        {
            _config = options.Value;
        }

        public IActionResult Index()
        {
            try
            {
                var client = new RestClient($"{_config.BrokerAPISandbox}{_config.AccountAPI}");

                RestRequest request = new();
                request.Method = Method.Post;
                request.AddHeader("Content-Type", "application/json");
                client.Authenticator = new HttpBasicAuthenticator(_config.BrokerAPIKey, _config.BrokerAPISecret);

                var account = new BrokerAccountModel
                {
                    enabled_assets = new List<string> { "us_equity" },
                    contact = new AccountContact
                    {
                        email_address = "anaya.upadhyay@hotmail.com",
                        phone_number = "984-285-5484",
                        street_address = new List<string> { "Pepsicola" },
                        unit = "ABC",
                        city = "Kathmandu",
                        state = "CA",
                        postal_code = "94401",
                        country = "USA"
                    },
                    identity = new AccountIdentity
                    {
                        given_name = "Anaya",
                        family_name = "Upadhyay",
                        date_of_birth = "1989-01-26",
                        tax_id = "666-44-4321",
                        tax_id_type = "NOT_SPECIFIED",
                        country_of_citizenship = "USA",
                        country_of_birth = "USA",
                        country_of_tax_residence = "USA",
                        funding_source = new List<string> { "employment_income" },
                    },
                    disclosures = new AccountDisclosure
                    {
                        is_control_person = false,
                        is_affiliated_exchange_or_finra = true,
                        is_politically_exposed = false,
                        immediate_family_exposed = false,
                        context = new List<AccountDisclosureContext>
                        {
                            new AccountDisclosureContext
                            {
                                context_type = "AFFILIATE_FIRM",
                                company_name = "Amnil",
                                company_street_address = new List<string> { "Jhamsikhel, Pulchowk"},
                                company_city = "Lalitpur",
                                company_state = "Bagmati",
                                company_country = "Nepal",
                                company_compliance_email = "anaya.upadhyay@hotmail.com"
                            }
                        }
                    },
                    agreements = new List<AccountAgreements>
                    {
                        new AccountAgreements
                        {
                            agreement = "customer_agreement",
                            signed_at = "2022-10-09T09:00:00Z",
                            ip_address = "202.51.76.39"
                        }
                    },
                    documents = new List<AccountDocument>
                    {
                        new AccountDocument
                        {
                            document_type = "identity_verification",
                            document_sub_type = "passport",
                            content = "/9j/Cg==",
                            mime_type = "image/jpeg"
                        }
                    },
                    trusted_contact = new AccountTrustedContact
                    {
                        given_name = "Anaya",
                        family_name = "Upadhyay",
                        email_address = "anaya.upadhyay@hotmail.com"
                    }
                };

                request.AddJsonBody(account);

                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    var result = JsonConvert.DeserializeObject<BrokerAccountModel>(response.Content);
                    return View(result);
                }
                else
                {
                    Console.WriteLine(response.Content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception => {ex}");
            }

            return View();
        }
    }
}
