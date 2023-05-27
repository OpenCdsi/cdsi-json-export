using OpenCdsi.Cases;
using OpenCdsi.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OpenCdsi.Export
{
    internal class Vaccine : scheduleSupportingDataCvxMap
    {
        public object Key { get; set; }

        public static Vaccine CreateFrom(scheduleSupportingDataCvxMap data)
        {
            foreach (var d in data.association)
            {
                d.antigen = (string)Keymaker.Create(d.antigen);
                d.associationEndAge = d.associationEndAge.Null();
                d.associationBeginAge = d.associationBeginAge.Null();
            }

            return new Vaccine
            {
                association = data.association,
                cvx = data.cvx,
                shortDescription = data.shortDescription,
                Key = Keymaker.Create(data.cvx)
            };
        }
    }
}
