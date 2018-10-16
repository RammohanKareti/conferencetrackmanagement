using ConferenceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceManagement.Interfaces
{
    interface ITrackSession
    {
        bool HasAnySlot();

        bool TryAddTalk(SessionTalk talk);

        bool RemoveTalk(SessionTalk talk);

        TimeSpan GetRemainingTime();
    }
}
