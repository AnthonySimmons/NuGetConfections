using NUnit.Framework;
using System;
using System.Diagnostics;
using NuGetConfections.Properties;
using System.Reflection;
using System.IO;
using Action = NuGetConfections.Enums.Action;
using ExitCode = NuGetConfections.Enums.ExitCode;

namespace NuGetConfections.FunctionalTests
{
    [TestFixture]
    public class NuGetConfectionsFTests
    {
        private string _testDirectory, _uncosolidatedTestDataPath, _consolidatedTestDataPath, _nugetConfectionsPath, _noPackageReferencesPath;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _testDirectory = GetTestDirectory();
            _uncosolidatedTestDataPath = Path.Combine(_testDirectory, "TestData\\Unconsolidated");
            _consolidatedTestDataPath = Path.Combine(_testDirectory, "TestData\\Consolidated");
            _noPackageReferencesPath = Path.Combine(_testDirectory, "TestData\\NoPackageReferences");
            _nugetConfectionsPath = Path.Combine(_testDirectory, "NuGetConfections.exe");
        }

        [Test]
        public void Unconsolidated_WithRepoArgTest()
        {
            int exitCode = ExecuteNuGetConfections($"{Action.VerifyConsolidation} Repository=TestData\\Unconsolidated", _testDirectory, out string stdErr, out string stdOut);

            const string expectedStdErr = @"Unconsolidated package versions found.
NUnit 3.9.0, is referenced from: 'TestData\Unconsolidated\Assembly3\packages.config'
NUnit 3.9.0, is referenced from: 'TestData\Unconsolidated\Assembly4\packages.config'
NUnit 3.7.1, is referenced from: 'TestData\Unconsolidated\Assembly5\packages.config'

";
            AssertUnconsolidatedResult(exitCode, stdErr, stdOut, expectedStdErr, string.Empty);
        }

        [Test]
        public void Unconsolidated_WithRepoArgAndIgnoredPackagesTest()
        {
            const string ignored = "NUnit";
            int exitCode = ExecuteNuGetConfections($"{Action.VerifyConsolidation} Repository=TestData\\Unconsolidated Ignored=\"{ignored}\"", _testDirectory, out string stdErr, out string stdOut);

            string expectedStdOut = string.Format(Resources.Package_0_IsIgnoredAndHasMultipleVersions, ignored) + Environment.NewLine;

            AssertConsolidatedWithIgnoredResult(exitCode, stdErr, stdOut, expectedStdOut);
        }

        [Test]
        public void Unconsolidated_WithoutRepoArgAndIgnoredPackagesTest()
        {
            const string ignored = "NUnit";
            int exitCode = ExecuteNuGetConfections($"{Action.VerifyConsolidation} Ignored=\"{ignored}\"", _testDirectory, out string stdErr, out string stdOut);

            string expectedStdOut = string.Format(Resources.Package_0_IsIgnoredAndHasMultipleVersions, ignored) + Environment.NewLine;

            AssertConsolidatedWithIgnoredResult(exitCode, stdErr, stdOut, expectedStdOut);
        }

        [Test]
        public void Unconsolidated_WithoutRepoArgTest()
        {
            string expectedStdErr = $@"Unconsolidated package versions found.{Environment.NewLine}" +
$@"NUnit 3.9.0, is referenced from: '{_testDirectory}\TestData\Unconsolidated\Assembly3\packages.config'{Environment.NewLine}" +
$@"NUnit 3.9.0, is referenced from: '{_testDirectory}\TestData\Unconsolidated\Assembly4\packages.config'{Environment.NewLine}" +
$@"NUnit 3.7.1, is referenced from: '{_testDirectory}\TestData\Unconsolidated\Assembly5\packages.config'{Environment.NewLine}{Environment.NewLine}";

            int exitCode = ExecuteNuGetConfections(Action.VerifyConsolidation.ToString(), _uncosolidatedTestDataPath, out string stdErr, out string stdOut);
            AssertUnconsolidatedResult(exitCode, stdErr, stdOut, expectedStdErr, string.Empty);
        }

        [Test]
        public void Consolidated_WithRepoArgsTest()
        {
            int exitCode = ExecuteNuGetConfections($"{Action.VerifyConsolidation} Repository=TestData\\Consolidated", _testDirectory, out string stdErr, out string stdOut);
            AssertConsolidatedResult(exitCode, stdErr, stdOut);
        }

        [Test]
        public void Consolidated_WithoutRepoArgsTest()
        {
            int exitCode = ExecuteNuGetConfections(Action.VerifyConsolidation.ToString(), _consolidatedTestDataPath, out string stdErr, out string stdOut);
            AssertConsolidatedResult(exitCode, stdErr, stdOut);
        }

        [Test]
        public void Consolidated_WithRepoAndIgnoredArgsTest()
        {
            const string ignored = "NUnit,AutoFixture";
            int exitCode = ExecuteNuGetConfections($"{Action.VerifyConsolidation} Repository=TestData\\Consolidated Ignored=\"{ignored}\"", _testDirectory, out string stdErr, out string stdOut);
            AssertConsolidatedResult(exitCode, stdErr, stdOut);
        }

        [Test]
        public void Consolidated_WithoutRepoAndIgnoredArgsTest()
        {
            const string ignored = "NUnit";
            int exitCode = ExecuteNuGetConfections($"{Action.VerifyConsolidation} Ignored=\"{ignored}\"", _consolidatedTestDataPath, out string stdErr, out string stdOut);
            AssertConsolidatedResult(exitCode, stdErr, stdOut);
        }


        [Test]
        public void PrintUsageTest()
        {
            int exitCode = ExecuteNuGetConfections(string.Empty, _consolidatedTestDataPath, out string stdErr, out string stdOut);

            Assert.AreEqual((int)ExitCode.PrintUsage, exitCode);
            Assert.AreEqual(string.Empty, stdErr);
            Assert.AreEqual(Resources.Usage + Environment.NewLine, stdOut);
        }

        [Test]
        public void NoPackageReferencesTest()
        {
            int exitCode = ExecuteNuGetConfections(Action.VerifyConsolidation.ToString(), _noPackageReferencesPath, out string stdErr, out string stdOut);
            Assert.AreEqual((int)ExitCode.Success, exitCode);
            Assert.AreEqual(string.Empty, stdErr);
            Assert.AreEqual(Resources.NoPackageReferencesFound + Environment.NewLine, stdOut);
        }
        
        private void AssertUnconsolidatedResult(int exitCode, string stdErr, string stdOut, string expectedStdErr, string expectedStdOut)
        {
            Assert.AreEqual((int)ExitCode.UnconsolidatedPackageFound, exitCode);
            Assert.AreEqual(expectedStdErr, stdErr);
            Assert.AreEqual(expectedStdOut, stdOut);
        }

        private void AssertConsolidatedResult(int exitCode, string stdErr, string stdOut)
        { 
            Assert.AreEqual((int)ExitCode.Success, exitCode);
            
            Assert.AreEqual(string.Empty, stdErr);
            Assert.AreEqual(Resources.AllPackagesConsolidated + Environment.NewLine, stdOut);
        }

        private void AssertConsolidatedWithIgnoredResult(int exitCode, string stdErr, string stdOut, string expectedStdOut)
        {
            Assert.AreEqual((int)ExitCode.Success, exitCode);

            Assert.AreEqual(string.Empty, stdErr);
            Assert.AreEqual(expectedStdOut, stdOut);
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
