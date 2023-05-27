using OpenCdsi.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCdsi.Export
{
    internal class Observation : scheduleSupportingDataObservation
    {
        public object Key { get; set; }

        public static Observation CreateFrom(scheduleSupportingDataObservation data)
        {
            return new Observation
            {
                clarifyingText = data.clarifyingText,
                codedValues = data.codedValues,
                contraindicationText = data.contraindicationText,
                group = data.group,
                indicationText = data.indicationText,
                observationCode = data.observationCode,
                observationTitle = data.observationTitle,
                Key = Keymaker.Create(data.observationCode)
            };
        }
    }
}
