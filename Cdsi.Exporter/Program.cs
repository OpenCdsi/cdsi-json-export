using System.IO;
using System.Text.Json;
using OpenCdsi.SupportingData;
using OpenCdsi.Testcases;
using OpenCdsi;

namespace OpenCdsi.Exporter
{
    class Program
    {
        public static string BasePath => @"..\..\..\..\Build";
        static void Main(string[] args)
        {
            Directory.CreateDirectory($"{Program.BasePath}/_data");

            Serializer.WriteJson($"{Program.BasePath}/_data/testcase.json", Library.Testcases.Export());
            Serializer.WriteJson($"{Program.BasePath}/_data/antigen.json", Data.Antigen.Export());
            Serializer.WriteJson($"{Program.BasePath}/_data/observation.json", Data.Schedule.observations.Export());
            Serializer.WriteJson($"{Program.BasePath}/_data/vaccine.json", Data.Schedule.Export());
        }
    }
}
