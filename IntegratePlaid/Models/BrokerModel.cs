using System.ComponentModel.DataAnnotations;

namespace IntegratePlaid.Models
{
    public class BrokerAccountModel
    {
        public Guid id { get; set; }
        public string account_number { get; set; }
        public string status { get; set; }
        public string crypto_status { get; set; }
        public string currency { get; set; }
        public string last_equity { get; set; }
        public string created_at { get; set; }
        [Required]
        public AccountContact contact { get; set; }
        [Required]
        public AccountIdentity identity { get; set; }
        [Required]
        public AccountDisclosure disclosures { get; set; }
        public List<AccountAgreements> agreements { get; set; }
        public AccountTrustedContact trusted_contact { get; set; }
        /// <summary>
        /// See https://alpaca.markets/docs/api-references/broker-api/accounts/accounts/#account-type
        /// </summary>
        public string account_type { get; set; }                    // For Response only
        public string trading_configurations { get; set; }          // For Response only
        public List<string> enabled_assets { get; set; }
        public List<AccountDocument> documents { get; set; }        // For Request only
    }

    public class AccountContact
    {
        [Required]
        public string email_address { get; set; }
        [Required]
        public string phone_number { get; set; }
        [Required]
        public List<string> street_address { get; set; }
        public string unit { get; set; }
        [Required]
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }
    }

    public class AccountIdentity
    {
        [Required]
        public string given_name { get; set; }
        [Required]
        public string family_name { get; set; }
        public string middle_name { get; set; }
        [Required]
        public string date_of_birth { get; set; }
        public string tax_id { get; set; }                      // For Request only
        /// <summary>
        /// Types are:
        /// USA_SSN, ARG_ART_CUIT, and many more..
        /// https://alpaca.markets/docs/api-references/broker-api/accounts/accounts/#tax-id-type
        /// </summary>
        public string tax_id_type { get; set; }                 // For Response only
        public string country_of_citizenship { get; set; }
        public string country_of_birth { get; set; }
        [Required]
        public string country_of_tax_residence { get; set; }
        /// <summary>
        /// Funding Sources are:
        /// employment_income, investments, inheritance, business_income, savings, family
        /// </summary>
        [Required]
        public List<string> funding_source { get; set; }
        public string visa_type { get; set; }                   // For Response only
        public string visa_expiration_date { get; set; }        // For Response only
        public string date_of_departure_from_usa { get; set; }  // For Response only
        public string permanent_resident { get; set; }          // For Response only
    }

    public class AccountDisclosure
    {
        [Required]
        public bool is_control_person { get; set; }
        [Required]
        public bool is_affiliated_exchange_or_finra { get; set; }
        [Required]
        public bool is_politically_exposed { get; set; }
        [Required]
        public bool immediate_family_exposed { get; set; }
        public bool is_discretionary { get; set; }              // For Response only
        public List<AccountDisclosureContext> context { get; set; }       // For Request only
    }

    public class AccountAgreements
    {
        [Required]
        public string agreement { get; set; }
        [Required]
        public string signed_at { get; set; }
        [Required]
        public string ip_address { get; set; }
        public string revision { get; set; }
    }

    public class AccountTrustedContact
    {
        [Required]
        public string given_name { get; set; }
        [Required]
        public string family_name { get; set; }
        public string email_address { get; set; }
    }

    public class AccountDisclosureContext
    {
        /// <summary>
        /// See https://alpaca.markets/docs/api-references/broker-api/accounts/accounts/#context-type
        /// </summary>
        [Required]
        public string context_type { get; set; }
        /// <summary>
        /// Required,
        /// If context_type is AFFILIATE_FIRM or CONTROLLED_FIRM
        /// </summary>
        public string company_name { get; set; }
        /// <summary>
        /// Required,
        /// If context_type is AFFILIATE_FIRM or CONTROLLED_FIRM
        /// </summary>
        public List<string> company_street_address { get; set; }
        /// <summary>
        /// Required,
        /// If context_type is AFFILIATE_FIRM or CONTROLLED_FIRM
        /// </summary>
        public string company_city { get; set; }
        /// <summary>
        /// Required,
        /// If context_type is AFFILIATE_FIRM or CONTROLLED_FIRM
        /// </summary>
        public string company_country { get; set; }
        /// <summary>
        /// Required,
        /// If company_country = USA
        /// </summary>
        public string company_state { get; set; }
        /// <summary>
        /// Required,
        /// If context_type is AFFILIATE_FIRM or CONTROLLED_FIRM
        /// </summary>
        public string company_compliance_email { get; set; }
        /// <summary>
        /// Required,
        /// If context_type is IMMEDIATE_FAMILY_EXPOSED
        /// </summary>
        public string given_name { get; set; }
        /// <summary>
        /// Required,
        /// If context_type is IMMEDIATE_FAMILY_EXPOSED
        /// </summary>
        public string family_name { get; set; }
    }

    public class AccountDocument
    {
        [Required]
        public string document_type { get; set; }
        public string document_sub_type { get; set; }
        /// <summary>
        /// base64 string
        /// </summary>
        [Required]
        public string content { get; set; }
        [Required]
        public string mime_type { get; set; }
    }
}