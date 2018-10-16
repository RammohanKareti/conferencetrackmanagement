using ConferenceManagement.Interfaces;
using ConferenceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceManagement.Services
{
    public class NonRecursiveScheduler : IConferenceScheduler
    {
        private List<SessionTalk> RegisteredTalks;

        private List<ConferenceTrack> Tracks;

        private readonly Random _randomizer;

        public NonRecursiveScheduler()
        {
            _randomizer = new Random();
        }

        public void Schedule(List<SessionTalk> talks, List<ConferenceTrack> tracks)
        {
            RegisteredTalks = talks;
            Tracks = tracks;

            if (TrySchedule())
            {
                Tracks.ForEach(t =>
                {
                    t.ScheduleBreak();
                    t.ScheduleNetworkingEvent();
                });
            }

        }


        private bool TrySchedule()
        {
            var candidateTalks = GetNonScheduledTalks();
            if (candidateTalks.Count == 0)
            {
                // Hooray success
                return true;
            }
            // Iterate each talk
            foreach (var talk in candidateTalks)
            {
                var randomOrderedTracks = GetRandomNumForTrack();
                foreach (var track in randomOrderedTracks)
                {
                    if (talk.IsScheduled)
                    {
                        continue;
                    }

                    // track is not full
                    if (track.HasAnySlot())
                    {
                        var randomSessionPicker = GetRandomSession();

                        if (randomSessionPicker == 1)
                        {
                            // try adding 
                            if (track.TryAddTalkToMorningSession(talk))
                            {

                            }
                            else
                            {
                                track.TryAddTalkToAfternoonSession(talk);
                            }

                        }
                        else
                        {
                            if (track.TryAddTalkToAfternoonSession(talk))
                            {

                            }
                            else
                            {
                                track.TryAddTalkToMorningSession(talk);
                            }
                        }
                    }
                }
            }

            var pendingCount = GetNonScheduledTalks().Count;

            if (pendingCount > 0)
            {
                // Looks like our random filling did not work, start from all over again
                ResetConference();
                return TrySchedule();
            }

            return true;
        }


        private void ResetConference()
        {
            Tracks.ForEach(tr => tr.Reset());
            RegisteredTalks.ForEach(t => t.IsScheduled = false);
        }


        private List<ConferenceTrack> GetRandomNumForTrack()
        {
            var tracksCount = Tracks.Count;
            var randomTracks = new List<ConferenceTrack>(tracksCount);
            var indexDictionary = new Dictionary<int, bool>();

            for (var i = 0; i < tracksCount; i++)
            {
                while (true)
                {
                    var randomValue = _randomizer.Next(0, tracksCount);
                    if (!indexDictionary.ContainsKey(randomValue))
                    {
                        indexDictionary.Add(randomValue, true);
                        randomTracks.Add(Tracks[randomValue]);
                        break;
                    }
                }
            }

            return randomTracks;
        }

        private int GetRandomSession()
        {
            return _randomizer.Next(2);
        }


        // Get Non scheduled talks
        private List<SessionTalk> GetNonScheduledTalks()
        {
            var nonScheduleTalks = RegisteredTalks.Where(s => !s.IsScheduled)
                .OrderByDescending(s => s.Duration).ToList();
            return nonScheduleTalks; 
        }
    }
} 