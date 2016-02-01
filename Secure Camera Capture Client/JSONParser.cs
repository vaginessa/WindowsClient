using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Secure_Camera_Capture_Client
{
    class JSONParser
    {
        public JSONParser (String JSONfile)
        {
            string testString = System.IO.File.ReadAllText(@"C:\Users\Nathan\Desktop\testing.json");
            string jsonString = Regex.Replace(testString, @"\s+", "").ToString();
            //New JsonObject
            JsonObject jO = new JsonObject();
            //Get id
            jO.id = Regex.Match(testString, "\"id\"[ :]+(\"[^\"]*\")").ToString();
            //Get dateCreated
            jO.date_created = Regex.Match(testString, "\"date_created\"[ :]+(\"[^\"]*\")").ToString();
            var stringLeftToIndex = testString.Length;
            var currentPosInString = 0;
            //Pull off the first characters
            currentPosInString += jO.id.Length + jO.date_created.Length + 2; //The extra 3 is the :'s and the {
            stringLeftToIndex -= currentPosInString;
            
            //Set list counters for levels of listing
            int yearLevel  = -1;
            int monthLevel = -1;
            int dayLevel   = -1;
            int hourLevel  = -1;

            //Starting at the year level
            while ( stringLeftToIndex > 0 )
            {
                //Get the year
                var year = jsonString.Substring(currentPosInString, 4);
                //Update counters
                currentPosInString += 4; stringLeftToIndex -= 4;

                //Create Object
                jO.year.Add(new JsonObject.Year(Convert.ToInt32(year)));
                yearLevel++;

                while (stringLeftToIndex > 0)
                {
                    //Get the month
                    currentPosInString += 3; stringLeftToIndex -= 3;
                    var month = "";
                    var monthBlock = jsonString.Substring(currentPosInString, 10);
                    month = Regex.Match(monthBlock, "\"([^\\)]+)\"").ToString();
                    month = Regex.Replace(month, "\"", "").ToString();
                    //Update counters
                    currentPosInString += month.Length + 2; stringLeftToIndex -= month.Length + 2;

                    //Create Object
                    jO.year.ElementAt(yearLevel).months.Add(new JsonObject.Month(getMonthInt(month)));
                    monthLevel++;

                    while (stringLeftToIndex > 0)
                    {
                        //Get the day
                        currentPosInString += 2; stringLeftToIndex -= 2;
                        var day = "";
                        var dayBlock = jsonString.Substring(currentPosInString, 4);
                        day = Regex.Match(dayBlock, "\"([^\\)]+)\"").ToString();
                        day = Regex.Replace(day, "\"", "").ToString();
                        //Update counters
                        currentPosInString += day.Length + 2; stringLeftToIndex -= day.Length + 2;

                        //Create Object
                        jO.year.ElementAt(yearLevel).months.ElementAt(monthLevel).days.Add(new JsonObject.Day(Convert.ToInt16(day)));
                        dayLevel++;

                        while ( stringLeftToIndex > 0)
                        {
                            //Get the hour
                            currentPosInString += 2; stringLeftToIndex -= 2;
                            var hour = "";
                            var hourBlock = jsonString.Substring(currentPosInString, 4);
                            hour = Regex.Match(hourBlock, "\"([^\\)]+)\"").ToString();
                            hour = Regex.Replace(hour, "\"", "").ToString();
                            //Update counters
                            currentPosInString += hour.Length + 3; stringLeftToIndex -= hour.Length + 3;

                            //Create object
                            jO.year.ElementAt(yearLevel).months.ElementAt(monthLevel).days.ElementAt(dayLevel).hours.Add(new JsonObject.Hour(Convert.ToInt16(hour)));
                            hourLevel++;

                            int hourBlockEnd = 0;
                            int i = currentPosInString;
                            while(stringLeftToIndex > 0)
                            {
                                if(jsonString.ElementAt(i) == ']')
                                {
                                    break;
                                }
                                else
                                {
                                    i++;
                                    hourBlockEnd++;                                   
                                }
                            }

                            hourBlock = jsonString.Substring(currentPosInString, hourBlockEnd);

                            //Get the objects in the hour block
                            var imageMatches = Regex.Match(hourBlock, "\\{([^\\}]+)\\}");

                            //Get first match
                            string minute = "";
                            string date_taken = "";
                            string file_name = "";
                            string method = "";

                            string firstMatch = imageMatches.ToString();
                            file_name = Regex.Match(firstMatch, "\"file_name\"[ :]+(\"[^\"]*\")").ToString().Substring(13);
                            date_taken = Regex.Match(firstMatch, "\"date_taken\"[ :]+(\"[^\"]*\")").ToString().Substring(14);
                            method = Regex.Match(firstMatch, "\"method\"[ :]+(\"[^\"]*\")").ToString().Substring(10);

                            file_name = Regex.Replace(file_name, "\"", "").ToString();
                            date_taken = Regex.Replace(date_taken, "\"", "").ToString();
                            method = Regex.Replace(method, "\"", "").ToString();

                            minute = date_taken.GetLast(2);

                            Console.WriteLine(file_name);
                            Console.WriteLine(date_taken);
                            Console.WriteLine(method);
                            Console.WriteLine(minute);


                            jO.year.ElementAt(yearLevel).months.ElementAt(monthLevel).days.ElementAt(dayLevel).hours.ElementAt(hourLevel).images.Add(new JsonObject.Image(Convert.ToInt16(minute), file_name, date_taken, getMethodInt(method)));
                            //Get more matches
                            string match = imageMatches.NextMatch().ToString();
                            string oldmatch = firstMatch;
                            while ( match != oldmatch )
                            {
                                file_name = Regex.Match(match, "\"file_name\"[ :]+(\"[^\"]*\")").ToString().Substring(13);
                                date_taken = Regex.Match(match, "\"date_taken\"[ :]+(\"[^\"]*\")").ToString().Substring(14);
                                method = Regex.Match(match, "\"method\"[ :]+(\"[^\"]*\")").ToString().Substring(10);

                                file_name = Regex.Replace(file_name, "\"", "").ToString();
                                date_taken = Regex.Replace(date_taken, "\"", "").ToString();
                                method = Regex.Replace(method, "\"", "").ToString();

                                minute = date_taken.GetLast(2);

                                jO.year.ElementAt(yearLevel).months.ElementAt(monthLevel).days.ElementAt(dayLevel).hours.ElementAt(hourLevel).images.Add(new JsonObject.Image(Convert.ToInt16(minute), file_name, date_taken, getMethodInt(method)));

                                //Update match
                                oldmatch = match;
                                match = imageMatches.NextMatch().ToString();
                            }
                            break;
                        }

                    }
                }
            }            

        }

        public Int16 getMonthInt(string month)
        {
            switch ( month ) {
                case "January":
                    return 1;
                case "February":
                    return 2;
                case "March":
                    return 3;
                case "April":
                    return 4;
                case "May":
                    return 5;
                case "June":
                    return 6;
                case "July":
                    return 7;
                case "August":
                    return 8;
                case "September":
                    return 9;
                case "October":
                    return 10;
                case "November":
                    return 11;
                case "December":
                    return 12;
                default:
                    return -1;
            }

        }

        public Int16 getMethodInt(string method)
        {
            switch (method)
            {
                case "timer":
                    return 1;
                case "motion":
                    return 2;
                default:
                    return -1;
            }
        }
    }
}
