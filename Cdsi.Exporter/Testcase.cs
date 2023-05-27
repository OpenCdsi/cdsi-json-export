using OpenCdsi.Cases;
using OpenCdsi.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCdsi.Export
{
    internal class Testcase : Cases.testcase
    {
        public object Key { get; set; }

        public static Testcase CreateFrom(testcase data)
        {
            return new Testcase
            {
                AssessmentDate = data.AssessmentDate,
                CdcTestId = data.CdcTestId,
                ChangedInVersion = data.ChangedInVersion,
                DateAdded = data.DateAdded,
                DateUpdated = data.DateUpdated,
                Doses = data.Doses,
                Evaluation = data.Evaluation,
                EvaluationTestType = data.EvaluationTestType,
                Forecast = data.Forecast,
                ForecastTestType = data.ForecastTestType,
                GeneralDescription = data.GeneralDescription,
                Patient = data.Patient,
                ReasonForChange = data.ReasonForChange,
                TestcaseName = data.TestcaseName,
                VaccineGroup = data.VaccineGroup,
                Key = Keymaker.Create(data.CdcTestId)
            };
        }
    }
}
