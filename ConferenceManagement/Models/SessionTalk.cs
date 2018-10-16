using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceManagement.Models
{
    public class SessionTalk
    {
        public SessionTalk()
        {
            TalkID = Guid.NewGuid(); 
        }

        public SessionTalk(string name, int durationInMinutes)
        {
            TalkName = name;
            TalkID = Guid.NewGuid();
            Duration = TimeSpan.FromMinutes(durationInMinutes);
        }

        public Guid TalkID { get; private set; }

        public DateTime Time { get; private set; }

        public string TimeString { get; private set; }

        public string TalkName { get; set; }

        public int Minutes { get; set; }

        public TimeSpan Duration { get; private set; }

        public bool IsScheduled { get; set; }

        public bool IsNoonSession { get; set; }

        public void Schedule(DateTime time)
        {
            IsScheduled = true;
            Time = time;
            TimeString = time.ToString(AppConstants.TimeFormat);
        }

        public void UnSchedule()
        {
            IsScheduled = false;
        }
    }
}