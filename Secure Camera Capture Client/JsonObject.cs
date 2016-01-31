using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secure_Camera_Capture_Client
{
    class JsonObject
    {
        public string id { get; set; }
        public string date_created { get; set; }

        public class Image
        {
            public Int16 minute { get; set; }
            public string file_name { get; set; }
            public string date_taken { get; set; }
        }

        public class Hour
        {
            //This is the hour in the day, 0-23
            public Int16 hour { get; set; }
            public IList<Image> images { get; set; }
        }

        public class Day
        {
            //This is the number of the day in the month 1-31ish
            public Int16 day_name { get; set; }
            public IList<Hour> hours { get; set; }
        }

        public class Month
        {
            //This is 1-12 January-December Respective
            public Int16 month_name { get; set; }
            public IList<Day> days { get; set; }

        }

        public class Year
        {
            //Int value of the year such as 2016
            public Int32 year_name { get; set; }
            public IList<Month> months { get; set; }
        }
    }
}
