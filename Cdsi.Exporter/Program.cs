using System.IO;
using System.Linq;
using System.Text.Json;

namespace OpenCdsi.Export
{
    class Program
    {
        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            // etc.
        };

        static void Main(string[] args)
        {
            Export("caselibrary", CaseLibrary.Cases.Values);
            Export("antigens", Cdsi.Antigens.Values.Select(Antigen.CreateFrom));
            Export("vaccines", Cdsi.Schedule.Vaccines);
            Export("groups", Cdsi.Schedule.VaccineGroups);
            Export("observations", Cdsi.Schedule.Observations);
            Export("conflicts", Cdsi.Schedule.LiveVirusConflicts);
        }

        static void Export(string basename, object obj)
        {
            var data = JsonSerializer.Serialize(obj, _options);
            File.WriteAllText(@"..\..\..\..\results\" + basename + ".json", data);
        }
    }
}
