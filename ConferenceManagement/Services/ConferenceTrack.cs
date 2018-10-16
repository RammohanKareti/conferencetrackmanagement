using ConferenceManagement.Interfaces;
using ConferenceManagement.Models;
using System;

namespace ConferenceManagement.Services
{
    public class ConferenceTrack : IConferenceTrack
    {
        public int TrackNumber { get; set; }

        private readonly SessionTimings _sessionTimings;

        public ConferenceTrack(SessionTimings sessionTimings, int number)
        {
            _sessionTimings = sessionTimings;
            Initialize();
            TrackNumber = number;
        }

        private void Initialize()
        {
            MorningSession = new TrackSession("Morning Session", _sessionTimings.MorningSessionSlot);
            AfternoonSession = new TrackSession("Afternoon Session", _sessionTimings.AfternoonSessionSlot);
            LunchBreak = _sessionTimings.LunchBreakSlot;
            NetworkingEvent = _sessionTimings.NetworkingEventStartTimeSlot;

        }


        public string Name { get; set; }

        public TrackSession MorningSession {get; set;}

        public Slot LunchBreak { get; set; }

        public TrackSession AfternoonSession { get; set; }

        public Slot NetworkingEvent { get; set; }

        public string LunchTimeString { get; private set; }

        public string NetworkStartTimeString { get; private set; }

        public bool TryAddTalkToMorningSession(SessionTalk talk)
        {
           return MorningSession.TryAddTalk(talk);
        }

        public bool TryAddTalkToAfternoonSession(SessionTalk talk)
        {
            return AfternoonSession.TryAddTalk(talk);
        }

        public void ScheduleBreak()
        {
            LunchBreak.FromTime = (LunchBreak.FromTime - MorningSession.RemainingTime);
            LunchTimeString = DateTime.Today.Add(LunchBreak.FromTime).ToString(AppConstants.TimeFormat);

        }

        public void ScheduleNetworkingEvent()
        {
            if (!(AfternoonSession.RemainingTime > (NetworkingEvent.ToTime - NetworkingEvent.FromTime)))
            {
                NetworkingEvent.FromTime = NetworkingEvent.ToTime - AfternoonSession.RemainingTime;
            }

            NetworkStartTimeString = DateTime.Today.Add(NetworkingEvent.FromTime).ToString(AppConstants.TimeFormat);
            
        }

        public bool HasAnySlot()
        {
            if (MorningSession.RemainingTime.TotalMinutes == 0
                && AfternoonSession.RemainingTime.TotalMinutes == 0)
            {
                return false;
            }
            return true;
        }

        public void RemoveTalklFromMorningSession(SessionTalk talk)
        {
            MorningSession.RemoveTalk(talk);
        }

        public void RemoveTalklFromAfternoonSession(SessionTalk talk)
        {
            AfternoonSession.RemoveTalk(talk);
        }


        public void Reset()
        {
            Initialize();
        }
    }
}