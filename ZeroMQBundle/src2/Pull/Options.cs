//----------------------------------------------------------------------------------
// Command line options
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace Pull
{
    internal class Options : CommandLineOptionsBase
    {
        public Options()
        {
            Delay = 0;
        }

        [OptionList("b", "bindEndPoints", Required = false, Separator = ';',
            HelpText = "List of end points to bind seperated by ';'")]
        public IList<string> BindEndPoints { get; set; }

        [Option("d", "delay", Required = false, HelpText = "Delay between messages (ms). Default = 0")]
        public int Delay { get; set; }

        [HelpOption(HelpText = "Dispaly this help screen.")]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = "Pull",
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            HandleParsingErrorsInHelp(help);
            help.AddPreOptionsLine("Usage: Pull.exe -b <bind endpoint list> [-d <time delay>]");
            help.AddOptions(this);

            return help;
        }

        private void HandleParsingErrorsInHelp(HelpText help)
        {
            if (LastPostParsingState.Errors.Count > 0)
            {
                var errors = help.RenderParsingErrorsText(this, 2); // indent with two spaces
                if (!string.IsNullOrEmpty(errors))
                {
                    help.AddPreOptionsLine(string.Concat(Environment.NewLine, "ERROR(S):"));
                    help.AddPreOptionsLine(errors);
                }
            }
        }
    }
}