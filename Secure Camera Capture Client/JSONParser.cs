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
            //New JsonObject
            JsonObject jO = new JsonObject();
            //Get id
            jO.id = Regex.Match(testString, "\"id\"[ :]+(\"[^\"]*\")").ToString();
            //Get dateCreated
            jO.date_created = Regex.Match(testString, "\"date_created\"[ :]+(\"[^\"]*\")").ToString();
            var stringLeftToIndex = testString.Length;
            var currentPosInString = 0;
            while ( stringLeftToIndex > 0 )
            {
                
            }            

        }

    }
}
