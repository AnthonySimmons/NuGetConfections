using NUnit.Framework;
using System;
using System.Diagnostics;
using NuGetConfections.Properties;
using System.Reflection;
using System.IO;

namespace NuGetConfections.FunctionalTests
{
    [TestFixture]
    public class NuGetConfectionsFTests
    {
        private string _testDirectory, _uncosolidatedTestDataPath, _consolidatedTestDataPath, _nugetConfectionsPath;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _testDirectory = GetTestDirectory();
            _uncosolidatedTestDataPath = Path.Combine(_testDirectory, "TestData\\Unconsolidated");
            _consolidatedTestDataPath = Path.Combine(_testDirectory, "TestData\\Consolidated");
            _nugetConfectionsPath = Path.Combine(_testDirectory, "NuGetConfections.exe");
        }

        [Test]
        public void Unconsolidated_WithRepoArgTest()
        {
            int exitCode = ExecuteNuGetConfections($"{Action.VerifyConsolidation} TestData\\Unconsolidated", _testDirectory, out string stdErr, out string stdOut);

            const string expectedStdErr = @"Unconsolidated package versions found.
NUnit.3.9.0, is referenced from: 'TestData\Unconsolidated\Assembly3\packages.config'
NUnit.3.9.0, is referenced from: 'TestData\Unconsolidated\Assembly4\packages.config'
NUnit.3.7.1, is referenced from: 'TestData\Unconsolidated\Assembly5\packages.config'

";
            AssertUnconsolidatedResult(exitCode, stdErr, stdOut, expectedStdErr);
        }

        [Test]
        public void Unconsolidated_WithoutRepoArgTest()
        {
            const string expectedStdErr = @"Unconsolidated package versions found.
NUnit.3.9.0, is referenced from: 'C:\workspace\NuGet-Confections\NuGetConfections\Test\NuGetConfections.FunctionalTests\bin\x86\Debug\TestData\Unconsolidated\Assembly3\packages.config'
NUnit.3.9.0, is referenced from: 'C:\workspace\NuGet-Confections\NuGetConfections\Test\NuGetConfections.FunctionalTests\bin\x86\Debug\TestData\Unconsolidated\Assembly4\packages.config'
NUnit.3.7.1, is referenced from: 'C:\workspace\NuGet-Confections\NuGetConfections\Test\NuGetConfections.FunctionalTests\bin\x86\Debug\TestData\Unconsolidated\Assembly5\packages.config'

";

            int exitCode = ExecuteNuGetConfections(Action.VerifyConsolidation.ToString(), _uncosolidatedTestDataPath, out string stdErr, out string stdOut);
            AssertUnconsolidatedResult(exitCode, stdErr, stdOut, expectedStdErr);
        }

        private void AssertUnconsolidatedResult(int exitCode, string stdErr, string stdOut, string expectedStdErr)
        { 
            Assert.AreEqual((int)ExitCode.UnconsolidatedPackageFound, exitCode);
            Assert.AreEqual(expectedStdErr, stdErr);
            Assert.AreEqual("", stdOut);
        }

        [Test]
        public void Consolidated_WithRepoArgsTest()
        {
            int exitCode = ExecuteNuGetConfections($"{Action.VerifyConsolidation} TestData\\Consolidated", _testDirectory, out string stdErr, out string stdOut);
            AssertConsolidatedResult(exitCode, stdErr, stdOut);
        }

        [Test]
        public void Consolidated_WithoutRepoArgsTest()
        {
            int exitCode = ExecuteNuGetConfections(Action.VerifyConsolidation.ToString(), _consolidatedTestDataPath, out string stdErr, out string stdOut);
            AssertConsolidatedResult(exitCode, stdErr, stdOut);
        }

        [Test]
        public void PrintUsageTest()
        {
            int exitCode = ExecuteNuGetConfections("", _consolidatedTestDataPath, out string stdErr, out string stdOut);

            Assert.AreEqual((int)ExitCode.PrintUsage, exitCode);
            Assert.AreEqual("", stdErr);
            Assert.AreEqual(Resources.Usage + Environment.NewLine, stdOut);
        }

        private void AssertConsolidatedResult(int exitCode, string stdErr, string stdOut)
        { 
            Assert.AreEqual((int)ExitCode.Success, exitCode);
            

            Assert.AreEqual("", stdErr);
            Assert.AreEqual("", stdOut);
        }

        private int ExecuteNuGetConfections(string args, string cwd, out string stdError, out string stdOutput)
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo(_nugetConfectionsPath, args)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = cwd
            };
            Process proc = Process.Start(procStartInfo);

            Assert.IsTrue(proc.WaitForExit(10000));

            stdError = proc.StandardError.ReadToEnd();
            stdOutput = proc.StandardOutput.ReadToEnd();
            return proc.ExitCode;
        }

        private string GetTestDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}
