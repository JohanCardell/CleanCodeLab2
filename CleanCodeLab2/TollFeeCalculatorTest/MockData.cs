using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculatorTest
{
    public class MockData
    {
        readonly string manyDates = "2020-06-30 06:00, 2020-06-30 06:29, 2020-06-30 07:29, 2020-06-30 08:52, 2020-06-30 10:13, 2020-06-30 10:25, 2020-06-30 11:04, 2020-06-30 15:29, 2020-06-30 16:30, 2020-06-30 16:50, 2020-06-30 18:00, 2020-06-30 19:00, 2020-06-30 19:30, 2020-06-30 21:30, 2020-07-01 00:00";
        readonly string tollFreePassages = "2020-06-30 00:05, 2020-06-30 06:34";
        readonly string passagesWithinTwoHours = "2020-06-30 16:15, 2020-06-30 16:30, 2020-06-30 17:00, 2020-06-30 17:30, 2020-06-30 18:0";
        readonly string passagesOverMultipleDays = "2020-06-26 16:15, 2020-06-29 16:30, 2020-06-30 17:00";
        
        public DateTime[] DateStringToDateTimeArray(string unparsedDates)
        {
            string[] dateStrings = unparsedDates.Split(", ");
            DateTime[] parsedDates = new DateTime[dateStrings.Length];
            for (int i = 0; i < parsedDates.Length; i++)
            {
                parsedDates[i] = DateTime.Parse(dateStrings[i]);
            }
            return parsedDates;
        }

        public DateTime[] GetManyPassages()
        {
            return DateStringToDateTimeArray(manyDates);
        }

        public DateTime[] GetTollFreePassages()
        {
            return DateStringToDateTimeArray(tollFreePassages);
        }

        public DateTime[] GetMultiplePassagesWithinTwoHours()
        {
            return DateStringToDateTimeArray(passagesWithinTwoHours);
        }
        public DateTime[] GetPassagesOverMultipleDays()
        {
            return DateStringToDateTimeArray(passagesOverMultipleDays);
        }
    }
}

