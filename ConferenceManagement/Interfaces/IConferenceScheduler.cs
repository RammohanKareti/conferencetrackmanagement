using ConferenceManagement.Models;
using ConferenceManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceManagement.Interfaces
{
    public interface IConferenceScheduler
    {
        void Schedule(List<SessionTalk> talks, List<ConferenceTrack> tracks);
    }
}
