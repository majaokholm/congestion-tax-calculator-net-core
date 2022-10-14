using congestion.calculator;
using congestion_tax_calculator_net_core.BO;
using congestion_tax_calculator_net_core.Services;
using Moq;
using System.Globalization;

namespace TestCongestionTaxCalculator
{
    public class Tests
    {
        private CongestionTaxCalculator congestionTaxCalculator;

        [SetUp]
        public void Setup()
        {
            var moqParameterService = new Mock<IParameterService>();
            congestionTaxCalculator = new CongestionTaxCalculator(moqParameterService.Object);
        }
    
        [Test]
        public void BaseTest()
        {
            DateTime[] dates = new DateTime[6];
            dates[0] = DateTime.Parse("2013-01-15 21:00:00", new CultureInfo("de-DE")); //0
            dates[1] = DateTime.Parse("2013-02-07 06:23:27", new CultureInfo("de-DE")); //8
            dates[2] = DateTime.Parse("2013-02-07 15:27:00", new CultureInfo("de-DE"));//13
            dates[3] = DateTime.Parse("2013-02-08 06:27:00", new CultureInfo("de-DE"));//8
            dates[4] = DateTime.Parse("2013-02-08 06:20:27", new CultureInfo("de-DE"));//8
            dates[5] = DateTime.Parse("2013-02-08 06:25:27", new CultureInfo("de-DE"));//8
            var result = congestionTaxCalculator.GetTax(Vehicles.Car, dates);

            Assert.That(result.Equals(29));                         
        }

        [Test]
        public void SameSixytMinsTest()
        {
            DateTime[] dates = new DateTime[3];
            dates[0] = DateTime.Parse("2013-02-08 07:02:00", new CultureInfo("de-DE"));//8
            dates[1] = DateTime.Parse("2013-02-08 06:25:27", new CultureInfo("de-DE"));//8
            dates[2] = DateTime.Parse("2013-02-08 06:20:27", new CultureInfo("de-DE"));//8
            
            var result = congestionTaxCalculator.GetTax(Vehicles.Car, dates);

            Assert.That(result.Equals(18));
        }

        [Test]
        public void TollFreeVechieleTest()
        {
            DateTime[] dates = new DateTime[3];
            dates[0] = DateTime.Parse("2013-02-08 06:27:00", new CultureInfo("de-DE"));//8
            dates[1] = DateTime.Parse("2013-02-08 06:25:27", new CultureInfo("de-DE"));//8
            dates[2] = DateTime.Parse("2013-02-08 06:20:27", new CultureInfo("de-DE"));//8

            var result = congestionTaxCalculator.GetTax(Vehicles.Busses, dates);

            Assert.That(result.Equals(0));

        }
    }
}