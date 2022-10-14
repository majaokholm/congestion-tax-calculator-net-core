using System;
using System.Text.Json;
using congestion.calculator;
using congestion_tax_calculator_net_core.BO;
using congestion_tax_calculator_net_core.Services;
using Newtonsoft.Json;

public class CongestionTaxCalculator
{
    private IParameterService _parameterService;
    public CongestionTaxCalculator(IParameterService parameterService) 
    {
        _parameterService = parameterService;
    }
    /**
    * Calculate the total toll fee 
    *
    * @param vehicle - the vehicle
    * @param dates   - date and time of all passes 
    * @return - the total congestion tax
    */

    public int GetTax(Vehicles vehicle, DateTime[] dates)
    {
        if (IsTollFreeVehicle(vehicle)) return 0;
        int totalFee = 0;
        
        DateTime[][] arrayOfSpecificDates = dates.GroupBy(x => x.Date).Select(s => s.ToArray()).ToArray();
        
        foreach (DateTime[] timeStampesperDay in arrayOfSpecificDates)
        {
            List<List<DateTime>> sixtyMinIntervals = RefactorTo60MinIntervals(timeStampesperDay);

            foreach(List<DateTime> sixtyMinInterval in sixtyMinIntervals)
            {
                totalFee += CalculateDailyTax(sixtyMinInterval);
            }            
        }
        return totalFee;
    }

    /**
    * Calculate the total toll fee for one day
    *
    * @param timeStamps   - date and time of all passes on one day
    * @return - the total congestion tax for that day
    */

    private int CalculateDailyTax(List<DateTime> timeStamps)
    {
        int dailyFee = 0;
        List<int> tempFees = new List<int>();
        foreach (DateTime timeStamp in timeStamps)
        {
            tempFees.Add(GetTollFee(timeStamp));
        }
        dailyFee = tempFees.Max();
        if (dailyFee > 60) dailyFee = 60;
        return dailyFee;
    }


    /**
    * Determines if toll free vehichle 
    *
    * @param vechicle   - enum defined in Business Object 'RequestBody
    * @return - bool
    */
    private bool IsTollFreeVehicle(Vehicles vechicle)
    {
        string vehicletype = vechicle.ToString();
        return vehicletype.Equals(TollFreeVehicles.Motorcycle.ToString()) ||
               vehicletype.Equals(TollFreeVehicles.Busses.ToString()) ||
               vehicletype.Equals(TollFreeVehicles.Emergency.ToString()) ||
               vehicletype.Equals(TollFreeVehicles.Diplomat.ToString()) ||
               vehicletype.Equals(TollFreeVehicles.Foreign.ToString()) ||
               vehicletype.Equals(TollFreeVehicles.Military.ToString());
    }

    /**
    * Gets toll rate for based on time
    *
    * @param date   - the time the vehicles passed
    * @return - tax fee as int
    */
    public int GetTollFee(DateTime date)
    {
        if (IsTollFreeDate(date)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        if (hour == 6 && minute >= 0 && minute <= 29) return 8;
        else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
        else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
        else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
        else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
        else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
        else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
        else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
        else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
        else return 0;
    }


    /**
    * Determines if toll free date 
    *
    * @param date   - the time the vehicles passed
    * @return - bool
    */
    private Boolean IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        if (year == 2013)
        {
            if (month == 1 && day == 1 ||
                month == 3 && (day == 28 || day == 29) ||
                month == 4 && (day == 1 || day == 30) ||
                month == 5 && (day == 1 || day == 8 || day == 9) ||
                month == 6 && (day == 5 || day == 6 || day == 21) ||
                month == 7 ||
                month == 11 && day == 1 ||
                month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
            {
                return true;
            }
        }
        return false;
    }

    /**
    * Refactors list of timestamps to list of list of 60 minutes intervals to simplyfy calculation of single charge rule 
    *
    * @param timeStampesperDay   - list of the time the vehicles passed during one day
    * @return - list of groupping of timestamps in 60 minutes intervals as List<List<DateTime>>
    */
    private List<List<DateTime>> RefactorTo60MinIntervals(DateTime[] timeStampesperDay)
    {
        List<List<DateTime>> sixtyMinIntervals = new List<List<DateTime>>();
        
        var startTicks = timeStampesperDay[0].Ticks;
        var goupsBySixtyMins = timeStampesperDay.GroupBy(date => (date.Ticks - startTicks) / TimeSpan.TicksPerHour);

        foreach (var goupsBySixtyMin in goupsBySixtyMins)
        {
            List<DateTime> sixtyMinInterval = new List<DateTime>();
            foreach (var singleTimeStamp in goupsBySixtyMin)
            {
                sixtyMinInterval.Add(new DateTime(singleTimeStamp.Ticks));
            }
            sixtyMinIntervals.Add(sixtyMinInterval);
        }

        return sixtyMinIntervals;
    }

    private enum TollFreeVehicles
    {
        Motorcycle = 0,
        Busses = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }
}