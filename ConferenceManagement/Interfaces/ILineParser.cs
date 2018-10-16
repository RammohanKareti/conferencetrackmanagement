using ConferenceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceManagement.Interfaces
{
    public interface ILineParser
    {
        bool TryParse(string lineInput, out SessionTalk sessionTalk);
    }
}
