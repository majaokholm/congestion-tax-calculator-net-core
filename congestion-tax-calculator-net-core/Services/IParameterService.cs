using congestion_tax_calculator_net_core.BO;

namespace congestion_tax_calculator_net_core.Services
{
    public interface IParameterService
    {
        IEnumerable<TaxRate> GetLocalTaxRates();
    }
}
