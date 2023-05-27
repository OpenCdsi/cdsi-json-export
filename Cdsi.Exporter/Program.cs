using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace OpenCdsi.Export
{
    class Program
    {
        private const string DefaultBuildFolder = @"..\..\..\..\results\";

        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            // etc.
        };

        static void Setup()
        {
            Directory.CreateDirectory(DefaultBuildFolder);
        }

        static void Main(string[] args)
        {
            Setup();

            Export("case", CaseLibrary.Cases.Values.Select(Testcase.CreateFrom));
            Export("antigens", Cdsi.Antigens.Values.Select(Antigen.CreateFrom));
            Export("vaccines", Cdsi.Schedule.Vaccines.Select(Vaccine.CreateFrom));
            Export("vaccine-groups", Cdsi.Schedule.VaccineGroups.Select(VaccineGroup.CreateFrom));
            Export("observations", Cdsi.Schedule.Observations.Select(Observation.CreateFrom));
            Export("live-virus-conflicts", Cdsi.Schedule.LiveVirusConflicts.Select(LiveVirusConflict.CreateFrom));
        }

        static void Export(string basename, object obj)
        {
            var data = JsonSerializer.Serialize(obj, _options);
            File.WriteAllText(DefaultBuildFolder + basename + ".json", data);
        }
    }
}
