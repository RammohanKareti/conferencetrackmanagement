using ConferenceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceManagement.Interfaces
{
    public interface IConferenceTrack
    {
        bool HasAnySlot();

        bool TryAddTalkToMorningSession(SessionTalk talk);

        bool TryAddTalkToAfternoonSession(SessionTalk talk);

        void ScheduleBreak();

        void ScheduleNetworkingEvent();

        void RemoveTalklFromMorningSession(SessionTalk talk);

        void RemoveTalklFromAfternoonSession(SessionTalk talk);

    }
}
