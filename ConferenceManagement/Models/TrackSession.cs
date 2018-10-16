using ConferenceManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceManagement.Models
{
    public class TrackSession : Slot, ITrackSession
    {
        public TrackSession(string name, TimeSpan fromTime, TimeSpan toTime)
            :base(fromTime,toTime)
        {
            Initialize(name);
        }

        public TrackSession(string name, Slot timeSlot)
            :base(timeSlot.FromTime, timeSlot.ToTime)
        {
            Initialize(name);
        }

        public string Name { get; set; }

        public TimeSpan RemainingTime { get; private set; }

        public List<SessionTalk> Talks { get; set; }

        private DateTime CurrentSlotPointer { get; set; }

        private void Initialize(string name)
        {
            Talks = new List<SessionTalk>();
            RemainingTime = (ToTime - FromTime);
            CurrentSlotPointer = DateTime.Today.Add(FromTime);
            Name = name;
        }

        public bool HasAnySlot()
        {
            if (RemainingTime.TotalMinutes == 0)
            {
                return false;
            }
            return true;
        }

        public bool TryAddTalk(SessionTalk talk)
        {
            if (RemainingTime >= talk.Duration)
            {
                talk.Schedule(CurrentSlotPointer);
                FillSlot(talk.Duration);
                Talks.Add(talk);
                return true;
            }
            return false;
        }

        public bool RemoveTalk(SessionTalk talk)
        {
            if (Talks.Any(t => t.TalkID == talk.TalkID))
            {
                talk.UnSchedule();
                ClearSlot(talk.Duration);
                Talks.Remove(talk);
                return true;
            }
            return false;
        }

        private void FillSlot(TimeSpan talkDuration)
        {
            RemainingTime = RemainingTime.Subtract(talkDuration);
            CurrentSlotPointer = CurrentSlotPointer.Add(talkDuration);
        }

        private void ClearSlot(TimeSpan talkDuration)
        {
            RemainingTime = RemainingTime.Add(talkDuration);
            CurrentSlotPointer = CurrentSlotPointer.Subtract(talkDuration);
        }

        public TimeSpan GetRemainingTime()
        {
            return RemainingTime;
        }
    }
}