using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiInstanceManager.Helpers
{
    public static class CMDLineHelper
    {
        public static Dictionary<String,List<String>> parseArguments(string[] input)
        {
            Log.Debug("Parsing arguments..");
            Dictionary<String, List<String>> commands = new Dictionary<String, List<String>>();

            string command = String.Empty;
            List<String> parameters = new List<String>();
            bool variablemode = false;
            for(var i = 0; i < input.Length; i++)
            {
                if(variablemode && input[i].Substring(0, 2).CompareTo("--")!=0)
                {
                    Log.Debug("Adding parameter: " + input[i]);
                    parameters.Add(input[i]);
                }
                if(input[i].Substring(0,2).CompareTo("--")==0)
                {
                    Log.Debug("Adding command...");
                    // Check if we have a previous command to save away first:
                    if(variablemode && command != String.Empty)
                    {
                        Log.Debug("Adding Command: " + command);
                        commands.Add(command, parameters);
                        parameters = new List<String>();    // Reset the parameters for good measure.
                        command = String.Empty;             // Reset the command for good measure.
                        variablemode = false;               // Doesn't really do anything, but it's for good measure.
                    }
                    // This is a "command" of sorts, new entry:
                    command = input[i].Substring(2, input[i].Length-2);
                    variablemode = true;
                }
            }
            // Lastly, save the last command:
            if (variablemode && command != String.Empty)
            {
                Log.Debug("Adding Command: " + command);
                commands.Add(command, parameters);
                parameters = new List<String>();    // Reset the parameters for good measure.
                command = String.Empty;             // Reset the command for good measure.
                variablemode = false;               // Doesn't really do anything, but it's for good measure.
            }
            return commands;
        }
        public static Dictionary<String, List<String>> GetArguments()
        {
            Log.Debug("Loading command-line arguments");
            var CommandLineArguments = parseArguments(Environment.GetCommandLineArgs());
            return CommandLineArguments;

        }
    }
}
