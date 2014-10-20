using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHD
{
    public class Settings
    {
        public const int DAY_TYPE_WORKDAY = 0;
        public const int DAY_TYPE_WORKDAY_HOLIDAY = 1;
        public const int DAY_TYPE_FREE_DAY = 2;
        public const int DAY_TYPE_DAILY = 3;

        public Settings() 
        {
        }

        public static int GetType(string dayType)
        {
            switch (dayType)
            { 
                case "Pracovné dni (školský rok)":
                case "Pracovné dni":
                    return 0;
                case "Pracovné dni (školské prázdniny)":
                    return 1;
                case "Voľné dni":
                    return 2;
                case "Denne":
                    return 3;
                default:
                    return 0;
            }
        }
    }
}

