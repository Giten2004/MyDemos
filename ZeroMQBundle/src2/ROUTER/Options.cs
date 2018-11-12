﻿//----------------------------------------------------------------------------------
// Command line options
// Author: Manar Ezzadeen
// Blog  : http://idevhawk.phonezad.com
// Email : idevhawk@gmail.com
//----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace ROUTER
{
    internal class Options : CommandLineOptionsBase
    {
        [OptionList("b", "bindEndPoints", Required = true, Separator = ';',
            HelpText = "List of end points to bind seperated by ';'")]
        public IList<string> BindEndPoints { get; set; }

        [Option("r", "replyMessage", Required = true,
            HelpText = "Message to send as reply that may contains replaceable macros: #msg# = received msg")]
        public string ReplyMessage { get; set; }

        [Option("d", "delay", Required = false, HelpText = "Delay between messages (ms). Default = 0")]
        public int Delay { get; set; }

        [HelpOption(HelpText = "Dispaly this help screen.")]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = "Rep Server",
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            HandleParsingErrorsInHelp(help);
            help.AddPreOptionsLine("Usage: Rep.exe -b <bind endpoint list> -r <reply msg pattern> [-d <time delay>]");
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