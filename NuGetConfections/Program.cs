using System;
using NuGetConfections.Properties;

namespace NuGetConfections
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                CommandOptions commandOptions = new CommandOptions(args);

                INuGetConfectionCommand command = CommandFactory.GetCommand(commandOptions);

                if(!command.TryRun(out string errorMessage))
                {
                    Console.Error.WriteLine(errorMessage);
                    return (int)ExitCode.UnconsolidatedPackageFound;
                }                   
                
                return (int)ExitCode.Success;
            }
            catch(PrintUsageException)
            {
                PrintUsage();
                return (int)ExitCode.PrintUsage;
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                PrintUsage();
                return (int)ExitCode.InvalidArguments;
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine(Resources.Usage);
        }
    }
}
