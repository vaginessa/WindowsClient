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
        public string method { get; set; }
        public List<Year> year { get; set; } 

        public JsonObject ()
        {
            year = new List<Year>();
        }

        public class Image
        {
            public Image(Int16 minute, string file_name, string date_taken, Int16 method)
            {
                this.minute = minute;
                this.file_name = file_name;
                this.date_taken = date_taken;
                this.method = method;
            }
            public Int16 minute { get; set; }
            public string file_name { get; set; }
            public string date_taken { get; set; }
            public Int16 method { get; set; }
        }

        public class Hour
        {
            public Hour(Int16 hour)
            {
                this.hour = hour;
                this.images = new List<Image>();
            }
            //This is the hour in the day, 0-23
            public Int16 hour { get; set; }
            public IList<Image> images { get; set; }
        }

        public class Day
        {
            public Day(Int16 day_name)
            {
                this.day_name = day_name;
                this.hours = new List<Hour>();
            }
            //This is the number of the day in the month 1-31ish
            public Int16 day_name { get; set; }
            public IList<Hour> hours { get; set; }
        }

        public class Month
        {
            public Month(Int16 month_name)
            {
                this.month_name = month_name;
                this.days = new List<Day>();
            }
            //This is 1-12 January-December Respective
            public Int16 month_name { get; set; }
            public IList<Day> days { get; set; }

        }

        public class Year
        {
            public Year(Int32 year_name)
            {
                this.year_name = year_name;
                this.months = new List<Month>();
            }
            
            //Int value of the year such as 2016
            public Int32 year_name { get; set; }
            public IList<Month> months { get; set; }
        }
    }
}
