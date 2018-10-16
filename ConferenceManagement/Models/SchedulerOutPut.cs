using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceManagement.Models
{
    public class SchedulerOutPut
    {
        public SchedulerOutPut()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public bool HasFailed { get; set; }

        public string ErrorMessage { get; set; }

        public List<Track> Tracks { get; set; }
        
    }
}