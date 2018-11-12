using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace WUProxy
{
    internal class Options : CommandLineOptionsBase
    {
        public Options()
        {
         
        }

        [OptionList("f", "frontend bindEndPoints", Required = true, Separator = '|',
            HelpText = "List of end points to bind seperated by '|'")]
        public IList<string> FrontendBindEndPoints { get; set; }

        [OptionList("b", "backend bindEndPoints", Required = true, Separator = '|',
            HelpText = "List of end points to bind seperated by '|'")]
        public IList<string> BackendBindEndPoints { get; set; }


        [HelpOption(HelpText = "Dispaly this help screen.")]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = "Publisher",
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            HandleParsingErrorsInHelp(help);
            help.AddPreOptionsLine(
                "Usage: Pub.exe -f <frontend bind endpoint list> ");
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