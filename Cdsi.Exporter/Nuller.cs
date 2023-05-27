using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCdsi.Export
{
    internal static class Nuller
    {
        public static string Null(this string value) { return value == "" ? null : value; }
        public static object Null(this object value) { return value; }
    }
}
