using OpenCdsi.Schedule;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCdsi.Export
{
    internal class LiveVirusConflict : scheduleSupportingDataLiveVirusConflict
    {
        public object Key { get; set; }

        public static LiveVirusConflict CreateFrom(scheduleSupportingDataLiveVirusConflict data)
        {
            return new LiveVirusConflict
            {
                previous=data.previous,
                current=data.current,
                conflictBeginInterval=data.conflictBeginInterval,
                minConflictEndInterval=data.minConflictEndInterval,
                conflictEndInterval=data.conflictEndInterval,
                Key = Keymaker.Create($"{data.current.cvx}{data.previous.cvx}")
            };
        }
    }
}
