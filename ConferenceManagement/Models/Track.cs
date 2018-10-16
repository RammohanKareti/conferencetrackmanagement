using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceManagement.Models
{
    /// <summary>
    /// Represents Each track in Conference
    /// </summary>
    public class Track
    {
        private readonly int StartMinutes;

        private readonly int LunchStartMinutes;

        private readonly int LunchEndMinutes;

        private readonly int MinNetworkMinutes;

        private readonly int MaxNetworkMinutes;

        private readonly int TotalAvaialbleMinutesPerDay;

        public Track(int number)
        {
            // Just accept this is important
            // Hmmm we can make it better tho
            StartMinutes = SessionConstraints.StartHour * AppConstants.MinutesPerHour;
            LunchStartMinutes = SessionConstraints.LunchStartHour * AppConstants.MinutesPerHour;
            LunchEndMinutes = SessionConstraints.LunchEndHour * AppConstants.MinutesPerHour;
            MinNetworkMinutes = SessionConstraints.MinNetworkStartHour * AppConstants.MinutesPerHour;
            MaxNetworkMinutes = SessionConstraints.MaxNetworkStartHour * AppConstants.MinutesPerHour;
            MorningRemainingMinutes = (LunchStartMinutes - StartMinutes);
            NoonRemainingMinutes = (MaxNetworkMinutes - LunchEndMinutes);
            TotalAvaialbleMinutesPerDay = MorningRemainingMinutes + NoonRemainingMinutes;
            MorningCurrentSlotPosition = StartMinutes;
            NoonCurrentSlotPostion = LunchEndMinutes;
            MorningSessionTalks = new List<SessionTalk>();
            AfterNoonSessionTalks = new List<SessionTalk>();
            TrackNumber = number;
        }

        public int TrackNumber { get; set; }

        private int MorningRemainingMinutes { get; set; }
        
        private int MorningCurrentSlotPosition { get; set; }

        private int NoonRemainingMinutes { get; set; }

        private int NoonCurrentSlotPostion { get; set; }

        public List<SessionTalk> MorningSessionTalks { get; set; }

        public string LunchAt { get; set; }

        public int LunchMinutes { get; set; }

        public string NetworkingEventAt { get; set; }

        public List<SessionTalk> AfterNoonSessionTalks { get; set; }

        /// <summary>
        /// Check if track has any time left (Hopefully not!)
        /// </summary>
        /// <returns></returns>
        public bool HasAnySlot()
        {
            if (MorningCurrentSlotPosition == LunchStartMinutes
                && NoonRemainingMinutes == MaxNetworkMinutes)
            {
                return false;
            }
            return true;
        }

       /// <summary>
       /// Just try add the talk gracefully
       /// </summary>
       /// <param name="talk"></param>
       /// <returns></returns>
        public bool TryAddTalk(SessionTalk talk)
        {
            // Let us try to fit the talk in the morning.
            if (MorningCurrentSlotPosition < LunchStartMinutes 
                && MorningRemainingMinutes >= talk.Minutes)
            {
                // Just do some basic calculation to look smart
                //talk.Time = DateTime.Today.AddMinutes(MorningCurrentSlotPosition);
                //talk.TimeString = talk.Time.ToString(AppConstants.TimeFormat);
                MorningSessionTalks.Add(talk);
                MorningCurrentSlotPosition += talk.Minutes;
                MorningRemainingMinutes -= talk.Minutes;
                return true;
            }

            // try adding to noon slot (Sleepy Time)
            if (NoonCurrentSlotPostion < MaxNetworkMinutes
                && NoonRemainingMinutes >= talk.Minutes)
            {
                // Again just being smart
                //talk.Time = DateTime.Today.AddMinutes(NoonCurrentSlotPostion);
                //talk.TimeString = talk.Time.ToString(AppConstants.TimeFormat);
                talk.IsNoonSession = true;
                AfterNoonSessionTalks.Add(talk);
                NoonCurrentSlotPostion += talk.Minutes;
                NoonRemainingMinutes -= talk.Minutes;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove the talk from Track, yay it has better place in another track
        /// </summary>
        /// <param name="talk"></param>
        public void RemoveTalk(SessionTalk talk)
        {
            if (talk.IsNoonSession)
            {
                if (AfterNoonSessionTalks.Any(t => t.TalkID == talk.TalkID))
                {
                    NoonCurrentSlotPostion -= talk.Minutes;
                    NoonRemainingMinutes = +talk.Minutes;
                    AfterNoonSessionTalks.Remove(talk);
                }
            }
            else
            {
                if (MorningSessionTalks.Any(t => t.TalkID == talk.TalkID))
                {
                    MorningCurrentSlotPosition -= talk.Minutes;
                    MorningRemainingMinutes += talk.Minutes;
                    MorningSessionTalks.Remove(talk);
                }
            }
        }

        // Finally Add Lunch at Networking Time slots
        public void AddLunchAndNetworkingTime()
        {
            LunchAt = DateTime.Today.AddMinutes(MorningCurrentSlotPosition)
                .ToString(AppConstants.TimeFormat);
            LunchMinutes = LunchEndMinutes - MorningCurrentSlotPosition;

            NetworkingEventAt = DateTime.Today.AddMinutes((NoonCurrentSlotPostion < MinNetworkMinutes)
                ? MinNetworkMinutes : NoonCurrentSlotPostion)
                .ToString(AppConstants.TimeFormat);
        }
        
    }


}