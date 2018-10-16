using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceManagement.Models
{
    public class SessionTimings
    {
        public Slot MorningSessionSlot { get; set; }

        public Slot AfternoonSessionSlot { get; set; }

        public Slot LunchBreakSlot { get; set; }

        public Slot NetworkingEventStartTimeSlot { get; set; }
    }
}