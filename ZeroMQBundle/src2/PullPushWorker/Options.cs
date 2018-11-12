//----------------------------------------------------------------------------------
// Command line options
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

using System;
using CommandLine;
using CommandLine.Text;

namespace PullPushWorker
{
    internal class Options : CommandLineOptionsBase
    {
        public Options()
        {
            Delay = 0;
        }

        [Option("l", "pullEndPoint", Required = true, HelpText = "Pull end point")]
        public string PullEndPoint { get; set; }

        [Option("s", "pushEndPoint", Required = true, HelpText = "Push end point")]
        public string PushEndPoint { get; set; }

        [Option("t", "rcvdMessageTag", Required = true,
            HelpText = "Tag the received msg that may contains replaceable macros: #msg# = received msg")]
        public string RcvdMessageTag { get; set; }

        [Option("d", "delay", Required = false, HelpText = "Delay between messages (ms). Default = 0")]
        public int Delay { get; set; }

        [HelpOption(HelpText = "Dispaly this help screen.")]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = "PullPushWorker",
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            HandleParsingErrorsInHelp(help);
            help.AddPreOptionsLine(
                "Usage: PullPushWorker.exe -l <Pull endpoint> -s -b <Pull endpoint> -t <Tag received Msg> [-d <time delay>]");
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