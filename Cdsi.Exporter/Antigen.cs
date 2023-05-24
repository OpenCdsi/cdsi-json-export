using OpenCdsi.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCdsi.Export
{
    internal class Antigen : antigenSupportingData
    {
        public string Name { get; set; }

        public static Antigen CreateFrom(antigenSupportingData data)
        {
            return new Antigen
            {
                contraindications = data.contraindications,
                immunity = data.immunity,
                series = data.series,
                Name = data.series[0].targetDisease
            };
        }
    }
}
