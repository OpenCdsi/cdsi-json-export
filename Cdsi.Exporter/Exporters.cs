﻿using OpenCdsi.SupportingData;
using OpenCdsi.Testcases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCdsi.Exporter
{
    internal static class Exporters
    {
        internal static object Export(this IDictionary<string, testcase> data)
        {
            var idx = data.Values.Select(x => new { Id = x.CdcTestId, Name = x.TestcaseName, Text = x.GeneralDescription, Group = x.VaccineGroup });

            Serializer.WriteJsonIndex($"{Program.BasePath}/testcases", idx);

            foreach (var tc in data.Values)
            {
                Serializer.WriteJsonIndex($"{Program.BasePath}/testcases/{tc.CdcTestId}", tc);

                var medicalData = new
                {
                    AssessmentDate = tc.AssessmentDate,
                    Dob = tc.Patient.DOB,
                    Gender = tc.Patient.Gender,
                    Observations = new object[0],
                    Doses = tc.Evaluation.AdministeredDoses.Select(x => new
                    {
                        Cvx = x.CVX,
                        Mvx = x.MVX,
                        VaccineName = x.VaccineName,
                        DateAdministered = x.DateAdministered,
                        LotExpiration = "",
                        Conditon = ""
                    })
                };
                Serializer.WriteJsonIndex($"{Program.BasePath}/testcases/{tc.CdcTestId}/medical", medicalData);
            }
            return idx;
        }

        internal static object Export(this IDictionary<string, antigenSupportingData> data)
        {
            var idx = data.Keys.Select(x => new { Id = x.ToId(), Name = x });
            Serializer.WriteJsonIndex($"{Program.BasePath}/antigens", idx);

            foreach (var kvp in data)
            {
                Serializer.WriteJsonIndex($"{Program.BasePath}/antigens/{kvp.Key.ToId()}", kvp.Value);
                Serializer.WriteJsonIndex($"{Program.BasePath}/antigens/{kvp.Key.ToId()}/series", kvp.Value.series);
                Serializer.WriteJsonIndex($"{Program.BasePath}/antigens/{kvp.Key.ToId()}/contraindications", kvp.Value.contraindications);
            }
            return idx;
        }

        internal static object Export(this ICollection<scheduleSupportingDataObservation> data)
        {
            var idx = data.Select(x => new { Id = x.observationCode, Name = x.observationTitle, Text = x.indicationText + " " + x.contraindicationText, Indicated = x.indicationText != "", Contraindicated = x.contraindicationText != "" });
            Serializer.WriteJsonIndex($"{Program.BasePath}/observations", idx);

            foreach (var obs in data)
            {
                Serializer.WriteJsonIndex($"{Program.BasePath}/observations/{obs.observationCode}", obs);
            }
            return idx;
        }

        internal static object Export(this scheduleSupportingData data)
        {
            var idx = data.cvxToAntigenMap.Select(x => new { Id = x.cvx, Name = x.shortDescription, Text = String.Join(",", x.association.Select(y => y.antigen).ToList()), Conflicts = data.liveVirusConflicts.Any(y => y.current.cvx == x.cvx) });
            Serializer.WriteJsonIndex($"{Program.BasePath}/vaccines", idx);

            foreach (var id in idx)
            {
                var vaccine = data.cvxToAntigenMap.Where(x => x.cvx == id.Id).First();
                Serializer.WriteJsonIndex($"{Program.BasePath}/vaccines/{id.Id}", vaccine);

                var conflicts = data.liveVirusConflicts.Where(x => x.current.cvx == id.Id);
                if (conflicts.Any())
                {
                    Serializer.WriteJsonIndex($"{Program.BasePath}/vaccines/{id.Id}/conflicts", conflicts);
                }

                var antigens = vaccine.association.Select(x => x.antigen.ToId());
                Serializer.WriteJsonIndex($"{Program.BasePath}/vaccines/{id.Id}/antigens", antigens);
            }

            return idx;
        }

        internal static string ToId(this string name)
        {
            return name.ToLower().Replace('/', '-').Replace(' ', '-');
        }
    }
}
