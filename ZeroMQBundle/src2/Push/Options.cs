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

namespace Push
{
    internal class Options : CommandLineOptionsBase
    {
        public Options()
        {
            Delay = 0;
            MaxMessage = -1;
        }

        [OptionList("b", "bindEndPoints", Required = false, Separator = ';',
            HelpText = "List of end points to bind seperated by ';'")]
        public IList<string> BindEndPoints { get; set; }

        [OptionList("m", "AlterMessages", Required = true, Separator = ';',
            HelpText =
                "List of alternavive messages to send seperated by ';'. It may contains macros: #nb# = number of the msg"
            )]
        public IList<string> AltMessages { get; set; }

        [Option("x", "MaxNbMessages", Required = false, HelpText = "Max nb message to send. Default -1 (unlimitted)")]
        public long MaxMessage { get; set; }

        [Option("d", "delay", Required = false, HelpText = "Delay between messages (ms). Default = 0")]
        public int Delay { get; set; }

        [HelpOption(HelpText = "Dispaly this help screen.")]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = "Push",
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            HandleParsingErrorsInHelp(help);
            help.AddPreOptionsLine(
                "Usage: Push.exe -b <bind endpoint list> -m <msgs to send> [-x <max nb msg>] [-d <time delay>]");
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