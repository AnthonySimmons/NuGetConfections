

namespace NuGetConfections.Commands
{
    internal interface INuGetConfectionCommand
    {
        bool TryRun(out string outputMessage);
    }
}
