using System.Text.Json;
using System.Text.Json.Nodes;
/*
    The program gives a comprehensive idea of using System.Text.Json to read from a config json file
    within the program
    AND use of System.Text.Json.Nodes to read from an API by feeding in country value to determine the 
    calling code.

    The advantage of using Json Nodes is that there is no need to create class types to extract the determined value
    from the API but it doesn't give much flexibility as it is tightly coupled !
*/
namespace DetermineCountryCode
{
    public class Config
    {
        public string? country { get; set; }
        public string? number { get; set; }
    }
    public class Program
    {
        public static async Task<string> GetCountryCode(string country, string number)
        {
            string apiUrl = $"https://jsonmock.hackerrank.com/api/countries?name={country}";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(apiUrl);

                    string responseData = await response.Content.ReadAsStringAsync();

                    //create a node to read the json values
                    JsonNode? jsonNode = JsonNode.Parse(responseData);

                    // Here data is an array of objects
                    JsonArray data = jsonNode!["data"]!.AsArray();

                    // CallingCodes is also an array type
                    JsonArray callingCodes = data[0]!["callingCodes"]!.AsArray();
                    int index = callingCodes.Count - 1;

                    string callingcode = callingCodes[index]!.ToString();

                    return $"+{callingcode} {number}";
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return "Error in country; No calling code";
        }
        public static async Task Main(string[] args)
        {
            string configFile = "config.json";
            Config? config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configFile));

            if (config == null || string.IsNullOrEmpty(config.country) || string.IsNullOrEmpty(config.number))
            {
                Console.WriteLine("Configuration file is invalid or missing.");
                return;
            }

            string answer = await GetCountryCode(config.country, config.number);
            Console.WriteLine("Number with countryCode : " + answer);

        }
    }
}