using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MultiInstanceManager
{

    public class Program
    {
        static void Main()
        {
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            AppDomain.CurrentDomain.AssemblyResolve += FindAssem;
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MultiInstanceManager());
            // RealProgram.Go();
        }
        static Assembly? FindAssem (object sender, ResolveEventArgs args)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string simpleName = new AssemblyName(args.Name).Name;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            string path = Path.GetFullPath("lib") + "\\" + simpleName + ".dll";
            string path2 = Path.GetFullPath("plugins\\ref") + "\\" + simpleName + ".dll";
            Debug.WriteLine("Finding assembly in : " + path);
            if (!File.Exists(path))
            {
                if (!File.Exists(path2))
                {
                    return null;
                }
                else
                {
                    return Assembly.LoadFrom(path2);
                }
            }
            return Assembly.LoadFrom(path);

        }
    }
    public class RealProgram
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        public static void Go()
        {
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MultiInstanceManager());
        }
    }
}
