using congestion_tax_calculator_net_core.BO;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace congestion_tax_calculator_net_core.Services
{
    public class ParameterService: IParameterService
    {
        private readonly ILogger<ParameterService> _logger;
        private readonly IConfiguration _configuration;
        private string _basepath;
        public ParameterService(ILogger<ParameterService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _basepath = Path.Combine(@"Paramaters", _configuration["AppSettings:city"]);            
        }

        public IEnumerable<TaxRate> GetLocalTaxRates()
        {            
            string fileName = "taxrates.json";
            Path.Combine(_basepath, fileName);
            string jsonTaxRates = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<IEnumerable<TaxRate>>(jsonTaxRates);
        }
        
    }
}
