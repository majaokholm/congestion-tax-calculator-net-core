using congestion.calculator;

namespace congestion_tax_calculator_net_core.BO
{
    public class RequestBody
    {
        public DateTime[] Dates { get; set; }
        public Vehicles Vehicle { get; set; }

        public RequestBody(DateTime[] dates, Vehicles vehicle)
        {
            Dates = dates;
            Vehicle = vehicle;
        }
    }

    public enum Vehicles
    {
        Motorcycle = 0,
        Busses = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5,
        Car = 6,
        Other = 7
    }
}
