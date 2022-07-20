using System.IO;
using System.Text.Json;
using OpenCdsi.SupportingData;
using OpenCdsi.Testcases;
using OpenCdsi;

namespace OpenCdsi.Exporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.CreateDirectory("_data/api-ids");

            Serializer.WriteJson("_data/api-ids/testcase.json", Library.Testcases.Export());
            Serializer.WriteJson("_data/api-ids/antigen.json", Data.Antigen.Export());
            Serializer.WriteJson("_data/api-ids/observation.json", Data.Schedule.observations.Export());
            Serializer.WriteJson("_data/api-ids/vaccine.json", Data.Schedule.Export());
        }
    }
}
