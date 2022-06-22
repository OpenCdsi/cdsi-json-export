using Cdsi.SupportingData;
using Cdsi.Testcases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cdsi.Exporter
{
    internal static class Exporters
    {
        internal static void Export(this IDictionary<string, testcase> data)
        {
            var idx = data.Values.Select(x => new { Id = x.CdcTestId, Name = x.TestcaseName, Description = x.GeneralDescription });

            Serializer.WriteAsJson(idx, "api\\testcases.json");

            foreach (var tc in data.Values)
            {
                Directory.CreateDirectory($"api\\testcases\\{tc.CdcTestId}");
                Serializer.WriteAsJson(tc, $"api\\testcases\\{tc.CdcTestId}.json");

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
                Serializer.WriteAsJson(medicalData, $"api\\testcases\\{tc.CdcTestId}\\medical.json");
            }
        }

        internal static void Export(this IDictionary<string, antigenSupportingData> data)
        {
            Directory.CreateDirectory("api\\antigens");

            Serializer.WriteAsJson(data.Keys.Select(x => x.ToId()), "api\\antigens.json");

            foreach (var kvp in data)
            {
                Serializer.WriteAsJson(kvp.Value, $"api\\antigens\\{kvp.Key.ToId()}.json");
                Directory.CreateDirectory($"api\\antigens\\{kvp.Key.ToId()}");
                Serializer.WriteAsJson(kvp.Value.series, $"api\\antigens\\{kvp.Key.ToId()}\\series.json");
                Serializer.WriteAsJson(kvp.Value.contraindications, $"api\\antigens\\{kvp.Key.ToId()}\\contraindications.json");
            }
        }

        internal static void Export(this ICollection<scheduleSupportingDataObservation> data)
        {
            Directory.CreateDirectory("api\\observations");

            var idx = data.Select(x => new { x.observationCode, x.observationTitle });
            Serializer.WriteAsJson(idx, "api\\observations.json");

            foreach (var obs in data)
            {
                Serializer.WriteAsJson(obs, $"api\\observations\\{obs.observationCode}.json");
            }
        }

        internal static void Export(this scheduleSupportingData data)
        {
            Directory.CreateDirectory($"api\\vaccines\\groups");

            var idx = data.cvxToAntigenMap.Select(x => new { Id = x.cvx, Description = x.shortDescription });
            Serializer.WriteAsJson(idx, "api\\vaccines.json");

            foreach (var id in idx)
            {
                var vaccine = data.cvxToAntigenMap.Where(x => x.cvx == id.Id).First();
                Serializer.WriteAsJson(vaccine, $"api\\vaccines\\{id.Id}.json");

                Directory.CreateDirectory($"api\\vaccines\\{id.Id}");

                var conflicts = data.liveVirusConflicts.Where(x => x.current.cvx == id.Id);
                Serializer.WriteAsJson(conflicts, $"api\\vaccines\\{id.Id}\\conflicts.json");

                var antigens = vaccine.association.Select(x => x.antigen.ToId());
                Serializer.WriteAsJson(antigens, $"api\\vaccines\\{id.Id}\\antigens.json");
            }

            var groups = data.vaccineGroups.Select(x => x.name);
            Serializer.WriteAsJson(groups.Select(x => x.ToId()), $"api\\vaccines\\groups.json");

            foreach (var groupName in groups)
            {
                var group = data.vaccineGroups.Where(x => x.name == groupName).First();
                Serializer.WriteAsJson(groups, $"api\\vaccines\\groups\\{groupName.ToId()}.json");

                Directory.CreateDirectory($"api\\vaccines\\groups\\{groupName.ToId()}");

                var groupAntigens = data.vaccineGroupToAntigenMap.Where(x => x.name == groupName);
                Serializer.WriteAsJson(groupAntigens.Select(x => x.name.ToId()), $"api\\vaccines\\groups\\{groupName.ToId()}\\antigens.json");
            }
        }

        internal static string ToId(this string name)
        {
            return name.ToLower().Replace('/', '-').Replace(' ', '-');
        }
    }
}
