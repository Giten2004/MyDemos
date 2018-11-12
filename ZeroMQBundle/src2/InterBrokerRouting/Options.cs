using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace InterBrokerRouting
{
    internal class Options : CommandLineOptionsBase
    {
        public Options()
        {
            CloudBackendPoints = new List<string>();
            StateFrontendPoints = new List<string>();
        }

        [Option("n", "brokerName", Required = true,
            HelpText = "Broker Name")]
        public string BrokerName { get; set; }

        [Option("f", "LocalFrontEndPoint", Required = true,
            HelpText = "LocalFrontEndPoint")]
        public string LocalFrontEndPoint { get; set; }

        [Option("b", "LocalBackendPoint", Required = true,
           HelpText = "LocalBackendPoint")]
        public string LocalBackendPoint { get; set; }

        [Option("F", "CloudFrontendPoint", Required = true,
          HelpText = "CloudFrontendPoint")]
        public string CloudFrontendPoint { get; set; }

        [OptionList("B", "CloudBackendPoints", Required = false, Separator = ';',
           HelpText = "CloudBackendPoints, List of end points to bind seperated by ';'")]
        public IList<string> CloudBackendPoints { get; set; }

        [OptionList("s", "StateFrontendPoints", Required = false,
          HelpText = "StateFrontendPoints")]
        public IList<string> StateFrontendPoints { get; set; }

        [Option("S", "StateBackendPoint", Required = true,
           HelpText = "StateBackendPoint")]
        public string StateBackendPoint { get; set; }

        [Option("M", "MonitorPoint", Required = true, HelpText = "MonitorPoint")]
        public string MonitorPoint { get; set; }

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
            //help.AddPreOptionsLine("Usage: Rep.exe -b <bind endpoint list> -r <reply msg pattern> [-d <time delay>]");
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
