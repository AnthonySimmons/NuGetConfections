
using System;
using System.Collections.Generic;

namespace NuGetConfections
{
    internal static class CommandFactory
    {
        private static readonly IDictionary<Action, Type> sCommandTypesByAction = new Dictionary<Action, Type>
        {
            [Action.VerifyConsolidation] = typeof(VerifyPackageConsolidationCommand)
        };

        public static INuGetConfectionCommand GetCommand(CommandOptions commandOptions)
        {
            return (INuGetConfectionCommand)Activator.CreateInstance(sCommandTypesByAction[commandOptions.Action], commandOptions);
        }
    }
}
