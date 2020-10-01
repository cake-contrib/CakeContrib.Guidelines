using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;
using Xunit.Abstractions;

namespace CakeContrib.Guidelines.Tasks.Tests
{
    public class E2eTests
    {
        private readonly ITestOutputHelper xUnitOutput;

        public E2eTests(ITestOutputHelper output)
        {
            xUnitOutput = output;
        }

        [Theory]
        [ClassData(typeof(Data))]
        public void Should_Pass_All_E3E_Tests_using_Framework(string projFile, Spec spec)
        {
            spec.Should().NotBeNull(); // to be save

            xUnitOutput.WriteLine("Description:" + spec.Description);
            xUnitOutput.WriteLine("");

            // restore
            var psi = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardOutput = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                ErrorDialog = false,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = "dotnet",
                Arguments = "restore " + projFile
            };

            using (Process process = Process.Start(psi))
            {
                process.WaitForExit();
            }

            psi = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                ErrorDialog = false,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = "dotnet",
#if NETCORE
                Arguments = "build " + projFile
#else
                Arguments = "msbuild " + projFile
#endif
            };

            var exitcode = -1;
            var allOutput = new List<string>();
            using (Process process = Process.Start(psi))
            {
                process.OutputDataReceived += (o, e) =>
                {
                    if (e.Data == null)
                    {
                        return;
                    }

                    xUnitOutput.WriteLine(e.Data);
                    allOutput.Add(e.Data);
                };

                process.BeginOutputReadLine();
                process.ErrorDataReceived += (o, e) =>
                {
                    if (e.Data == null)
                    {
                        return;
                    }
                    xUnitOutput.WriteLine("ERROR: " + e.Data);
                    allOutput.Add(e.Data);
                };
                process.BeginErrorReadLine();

                process.WaitForExit();
                xUnitOutput.WriteLine("Exit code:" + process.ExitCode);
                exitcode = process.ExitCode;
            }

            // then
            exitcode.Should().Be(spec.ExitCode, "that is the expected exit-code");

            var errorLines = allOutput.Where(x => x.IndexOf(" error ", StringComparison.OrdinalIgnoreCase) > -1).ToList();
            errorLines.Should().HaveCount(spec.Errors.Count(), $"that is the expected number of errors");
            foreach (var err in spec.Errors)
            {
                errorLines.Should().Contain(x => x.IndexOf(err, StringComparison.Ordinal) > -1, $"expected error {err}");
            }

            var warnLines = allOutput.Where(x => x.IndexOf(" warning ", StringComparison.OrdinalIgnoreCase) > -1).ToList();
            warnLines.Should().HaveCount(spec.Warnings.Count(), $"that is the expected number of warnings");
            foreach (var warn in spec.Warnings)
            {
                warnLines.Should().Contain(x => x.IndexOf(warn, StringComparison.OrdinalIgnoreCase) > -1, $"expected warning {warn}");
            }
        }

        private class Data : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var assembly = typeof(E2eTests).Assembly;
                // assembly.Location returns the shadowCopy...
                var assemblyLocation = new Uri(assembly.CodeBase).LocalPath;
                var srcFolder = Directory.GetParent(assemblyLocation);
                while (!srcFolder.Name.Equals("src", StringComparison.OrdinalIgnoreCase))
                {
                    srcFolder = srcFolder.Parent;
                    if (srcFolder == null)
                    {
                        throw new FileNotFoundException("unable to find project-source folder.");
                    }
                }
                var e2eFolder = srcFolder.Parent.GetDirectories().SingleOrDefault(d => d.Name.Equals("e2e-tests", StringComparison.OrdinalIgnoreCase));
                if (e2eFolder == null)
                {
                    throw new FileNotFoundException("unable to find e2e-tests folder.");
                }
                var testProjects = e2eFolder.GetFiles("*.csproj", SearchOption.AllDirectories);
                foreach (var proj in testProjects)
                {
                    var specFile = proj.Directory.GetFiles("spec.json").SingleOrDefault();
                    Spec spec = null;
                    if (specFile != null)
                    {
                        spec = JsonConvert.DeserializeObject<Spec>(File.ReadAllText(specFile.FullName));
                    }

                    yield return new object[] { proj.FullName, spec };
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class Spec
        {
            public string Description { get; set; }
            public int ExitCode { get; set; }
            public string[] Errors { get; set; }
            public string[] Warnings { get; set; }
        }
    }
}
