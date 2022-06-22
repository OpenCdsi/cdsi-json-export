using System.IO;
using System.Text.Json;
using Cdsi.SupportingData;
using Cdsi.Testcases;
using Cdsi;

namespace Cdsi.Exporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.CreateDirectory("_data/api-ids");

            Serializer.WriteJson("_data/api-ids/testcase.json", Library.Testcases.Export());
            Serializer.WriteJson("_data/api-ids/antigen.json", Data.Antigen.Export());
            Serializer.WriteJson("_data/api-ids/observation.json", Data.Schedule.observations.Export());

            var scheduleIds = Data.Schedule.Export();
            Serializer.WriteJson("_data/api-ids/vaccine.json", scheduleIds.Item1);
            Serializer.WriteJson("_data/api-ids/group.json", scheduleIds.Item2);
        }
    }
}
