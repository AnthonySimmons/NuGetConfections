

using System;
using System.Collections.Generic;
using System.Linq;
using NuGetConfections.Properties;

namespace NuGetConfections
{
    internal class CommandOptions
    {
        public CommandOptions(string[] args)
        {
            if(args.Length == 0)
            {
                throw new PrintUsageException();
            }

            try
            {
                Action = (Action)Enum.Parse(typeof(Action), args[0], ignoreCase: true);
                Options = args.ToList().GetRange(1, args.Length - 1);
            }
            catch
            {
                throw new ArgumentException(Resources.InvalidArguments, nameof(args));
            }
        }
        

        public Action Action { get; }

        public IEnumerable<string> Options { get; }
    }
}
