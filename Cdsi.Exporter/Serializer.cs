using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cdsi.Exporter
{
    internal static class Serializer
    {
      static  JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            // etc.
        };

        internal static void WriteAsJson(object thing, string filename)
        {
            var data = JsonSerializer.Serialize(thing, Options);
            File.WriteAllText(filename, data);
        }
    }
}
