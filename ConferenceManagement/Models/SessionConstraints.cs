using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ConferenceManagement.Models
{
    public static class SessionConstraints
    {
        static SessionConstraints()
        {
            // Load Constraints from configuration

            var appSettings = ConfigurationManager.AppSettings;

            if (int.TryParse(appSettings["SessionStartHour"], out int startHour))
            {
                StartHour = startHour;
            }

            if (int.TryParse(appSettings["SessionLunchStartHour"], out int lunchStartHour))
            {
                LunchStartHour = lunchStartHour;
            }

            if (int.TryParse(appSettings["SessionLunchEndHour"], out int lunchEndHour))
            {
                LunchEndHour = lunchEndHour;
            }

            if (int.TryParse(appSettings["SessionNetworkMinHour"], out int minNetworkStartHour))
            {
                MinNetworkStartHour = minNetworkStartHour;
            }

            if (int.TryParse(appSettings["SessionNetworkMaxHour"], out int maxNetworkStartHour))
            {
                MaxNetworkStartHour = maxNetworkStartHour;
            }

            TotalAvailableMinutes = ((LunchStartHour - startHour)
                + (MaxNetworkStartHour - lunchEndHour)) * AppConstants.MinutesPerHour;

        }

        public static int TotalAvailableMinutes { get; private set; }

        public static int StartHour { get; private set; }

        public static int LunchStartHour { get; private set; }

        public static int LunchEndHour { get; private set; }

        public static int MinNetworkStartHour { get; private set; }

        public static int MaxNetworkStartHour { get; private set; }
    }

}