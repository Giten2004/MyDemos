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

namespace Sub
{
    internal class Options : CommandLineOptionsBase
    {
        public Options()
        {
            SubscriptionPrefixes = new List<string>();
        }

        [OptionList("c", "connectEndPoints", Required = true, Separator = ';',
            HelpText = "List of end points to connect seperated by ';'")]
        public IList<string> ConnectEndPoints { get; set; }

        [OptionList("s", "subscriptionPrefixes", Required = false, Separator = ';',
            HelpText = "List of prefix filters seperated by ';'. Filtering the arrived messages. Default is empty")]
        public IList<string> SubscriptionPrefixes { get; set; }

        [Option("d", "delay", Required = false, HelpText = "Delay between messages (ms). Default = 0")]
        public int Delay { get; set; }

        [HelpOption(HelpText = "Dispaly this help screen.")]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = "Subscriber",
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            HandleParsingErrorsInHelp(help);
            help.AddPreOptionsLine(
                "Usage: Sub.exe -c <connect endpoint list> [-s <subscrp. prefixes>] [-d <time delay>]");
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