using Spectre.Console.Cli;
using System.Globalization;

namespace EcuDiagSimLuaDynamization
{
    internal class Program
    {

            static int Main(string[] args)
            {
                // Set the culture to English
                //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            var app = new CommandApp();
            app.Configure(config =>
            {
                config.AddCommand<ProcessFilesCommand>("process")
                    .WithDescription("Processes files with the specified protocol and saves them in the output directory.");
            });
            
            return app.Run(args);
            }

    }
}
