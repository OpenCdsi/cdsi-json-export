using OpenCdsi.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCdsi.Export
{
    internal class VaccineGroup : scheduleSupportingDataVaccineGroup
    {
        public object Key { get; set; }

        public static VaccineGroup CreateFrom(scheduleSupportingDataVaccineGroup data)
        {
            return new VaccineGroup
            {
                name = data.name,
                administerFullVaccineGroup = data.administerFullVaccineGroup.Null(),
                Key = Keymaker.Create(data.name)
            };
        }
    }
}
