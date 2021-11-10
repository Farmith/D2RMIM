using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiInstanceManager.Extensions
{
    public static class StringExtensions
    {
        public static void RemoveFileExt(this string input,string directory="")
        {
            var path = "";
            if(directory.Length > 0)
            {
                if(directory.Substring(directory.Length-1,1).CompareTo("\\") != 0)
                {
                    path += "\\";
                }
                path = directory;
            } else
            {

            }
//            System.IO.Path.GetFileNameWithoutExtension();
        }
    }
}
