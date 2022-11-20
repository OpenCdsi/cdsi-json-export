using System.IO;
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
            Export("caselibrary", CaseLibrary.Cases);
            Export("antigens", Cdsi.Antigens);


            Export("vaccines", Cdsi.Schedule.Vaccines);
            Export("groups", Cdsi.Schedule.VaccineGroups);
            Export("observations", Cdsi.Schedule.Observations);
            Export("conflicts", Cdsi.Schedule.LiveVirusConflicts);
        }

        static void Export(string basename, object obj)
        {
            var data = JsonSerializer.Serialize(obj, _options);
            File.WriteAllText(basename + ".json", data);
        }
    }
}
