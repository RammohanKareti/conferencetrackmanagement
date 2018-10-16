using ConferenceManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceManagement.Models
{
    public class ScheduledResult
    {
        public ScheduledResult()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public bool HasFailed { get; set; }

        public string ErrorMessage { get; set; }

        public List<ConferenceTrack> Tracks { get; set; }
    }
}