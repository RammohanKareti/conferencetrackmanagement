using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceManagement.Models
{
    public class Slot
    {
        public Slot(TimeSpan fromTime, TimeSpan toTime)
        {
            FromTime = fromTime;
            ToTime = toTime;
        }

        public TimeSpan FromTime { get; set; }

        public TimeSpan ToTime { get; set; }
    }
}