using ConferenceManagement.Interfaces;
using ConferenceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ConferenceManagement.Services
{
    public class InputLineParser : ILineParser
    {
        private const string MinLiteralStringToken = "min";
        private const string LightningTalkToken = "lightning";
        private const int LightningTalkMinutes = 5;

        public bool TryParse(string lineInput, out SessionTalk sessionTalk)
        {
            sessionTalk = null;
            if (string.IsNullOrWhiteSpace(lineInput))
            {
                return false;
            }

            var parsedLine = ParseLine(lineInput);

            // Looks like lighting but not quite right
            if (parsedLine.IsLightning
                && !string.Equals(parsedLine.Value,
                    LightningTalkToken, StringComparison.InvariantCultureIgnoreCase))
            {
                // evil
                return false;
            }

            int minutes = 0;

            // Number in another galaxy ?
            if (!parsedLine.IsLightning &&
                !int.TryParse(parsedLine.Value, out minutes)) // invalid number
            {
                //evil
                return false;
            }
            
            sessionTalk = new SessionTalk(parsedLine.Name,
                parsedLine.IsLightning ? LightningTalkMinutes : minutes);

            return true;
        }

        /// <summary>
        /// Parsing the Line (No magic)
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private ParsedLine ParseLine(string line)
        {
            var parsedString = new ParsedLine();
            var lineLength = line.Length;
            // read last three characters
            var lastThreeCharcters = line.Substring(lineLength - MinLiteralStringToken.Length);
            var builder = new StringBuilder();
            // Must be Lightning talk
            if (!string.Equals(lastThreeCharcters, MinLiteralStringToken))
            {
                parsedString.IsLightning = true;
                builder.Insert(0, lastThreeCharcters);
            }

            // It does not matter parse through till the start get clear picture
            var pos = MinLiteralStringToken.Length + 1;
            var currIndex = lineLength - pos;
            while (currIndex >= 0)
            {
                var currChar = line[lineLength - pos];
                if (currChar == ' ') // break when you find space ;)
                {
                    break;
                }
                builder.Insert(0, currChar);
                pos++;
                currIndex = lineLength - pos;
            }
            parsedString.Value = builder.ToString();
            // Evil data check 
            parsedString.Name = line.Substring(0, currIndex >= 0 ? currIndex : 0);
            return parsedString; ;
        }
    }
}