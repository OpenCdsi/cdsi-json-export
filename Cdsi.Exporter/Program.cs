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
            Directory.CreateDirectory("api");

            Library.Testcases.Export();
            Data.Antigen.Export();
            Data.Schedule.observations.Export();
            Data.Schedule.Export();
        }
    }
}
