using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiInstanceManager.Helpers
{
    public static class Sanity
    {
        public static string RemoveFileExt(string input)
        {
            if (input.Contains("."))
            {
                var parts = input.Split('.');
                return parts[0];
            }
            return input;
        }
    }
}
