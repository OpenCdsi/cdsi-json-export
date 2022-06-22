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

            Serializer.WriteJsonIndex("api\\testcases", idx);

            foreach (var tc in data.Values)
            {
                Serializer.WriteJsonIndex($"api\\testcases\\{tc.CdcTestId}", tc);

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
                Serializer.WriteJsonIndex($"api\\testcases\\{tc.CdcTestId}\\medical", medicalData);
            }
        }

        internal static void Export(this IDictionary<string, antigenSupportingData> data)
        {
            var idx = data.Keys.Select(x => x.ToId());
            Serializer.WriteJsonIndex("api\\antigens", idx);

            foreach (var kvp in data)
            {
                Serializer.WriteJsonIndex($"api\\antigens\\{kvp.Key.ToId()}", kvp.Value);
                Serializer.WriteJsonIndex($"api\\antigens\\{kvp.Key.ToId()}\\series", kvp.Value.series);
                Serializer.WriteJsonIndex($"api\\antigens\\{kvp.Key.ToId()}\\contraindications", kvp.Value.contraindications);
            }
        }

        internal static void Export(this ICollection<scheduleSupportingDataObservation> data)
        {
            var idx = data.Select(x => new { x.observationCode, x.observationTitle });
            Serializer.WriteJsonIndex("api\\observations", idx);

            foreach (var obs in data)
            {
                Serializer.WriteJsonIndex($"api\\observations\\{obs.observationCode}", obs);
            }
        }

        internal static void Export(this scheduleSupportingData data)
        {
            var idx = data.cvxToAntigenMap.Select(x => new { Id = x.cvx, Description = x.shortDescription });
            Serializer.WriteJsonIndex("api\\vaccines", idx);

            foreach (var id in idx)
            {
                var vaccine = data.cvxToAntigenMap.Where(x => x.cvx == id.Id).First();
                Serializer.WriteJsonIndex($"api\\vaccines\\{id.Id}", vaccine);

                var conflicts = data.liveVirusConflicts.Where(x => x.current.cvx == id.Id);
                Serializer.WriteJsonIndex($"api\\vaccines\\{id.Id}\\conflicts", conflicts);

                var antigens = vaccine.association.Select(x => x.antigen.ToId());
                Serializer.WriteJsonIndex($"api\\vaccines\\{id.Id}\\antigens", antigens);
            }

            var groups = data.vaccineGroups.Select(x => x.name);
            Serializer.WriteJsonIndex($"api\\vaccines\\groups", groups.Select(x => x.ToId()));

            foreach (var groupName in groups)
            {
                var group = data.vaccineGroups.Where(x => x.name == groupName).First();
                Serializer.WriteJsonIndex($"api\\vaccines\\groups\\{groupName.ToId()}", group);

                var groupAntigens = data.vaccineGroupToAntigenMap.Where(x => x.name == groupName);
                Serializer.WriteJsonIndex($"api\\vaccines\\groups\\{groupName.ToId()}\\antigens", groupAntigens.Select(x => x.name.ToId()));
            }
        }

        internal static string ToId(this string name)
        {
            return name.ToLower().Replace('/', '-').Replace(' ', '-');
        }
    }
}
