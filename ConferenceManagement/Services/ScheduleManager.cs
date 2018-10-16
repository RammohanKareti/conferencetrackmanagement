using ConferenceManagement.Interfaces;
using ConferenceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceManagement.Services
{
    public class ScheduleManager
    {
        private readonly IConferenceScheduler _conferenceScheduler;

        public ScheduleManager(IConferenceScheduler conferenceScheduler)
        {
            _conferenceScheduler = conferenceScheduler;
        }

        public void Schedule(List<SessionTalk> registeredTalks, List<ConferenceTrack> tracks)
        {
            _conferenceScheduler.Schedule(registeredTalks, tracks);
        }
    }
}