

namespace NuGetConfections
{
    internal interface INuGetConfectionCommand
    {
        bool TryRun(out string outputMessage);
    }
}
