using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;
using System.Threading;

namespace BSS.Launcher
{
    public static class xProcess
    {
        public struct Result
        {
            public Result(Int32 exitCode, String stdout, String stderr)
            {
                ExitCode = exitCode;
                this.stdout = stdout;
                this.stderr = stderr;
            }

            public Int32 ExitCode;
            public String stdout;
            public String stderr;
        }

        ///<summary>Launches executables.</summary>
        ///<exception cref="FileLoadException"></exception>
        public static Result? Run(String path, String args = null, Boolean RunAs = false, Boolean redirectOutput = false, Boolean hiddenExecute = false, Boolean waitForExit = false, String workingDirectory = null)
        {
            using Process process = new();

            process.StartInfo.FileName = path;

            if (RunAs)
            {
                process.StartInfo.Verb = "runas";
            }

            if (hiddenExecute)
            {
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;
            }

            if (redirectOutput)
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
            }
            else
            {
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.RedirectStandardError = false;
            }

            if (workingDirectory != null)
            {
                process.StartInfo.WorkingDirectory = workingDirectory;
            }

            if (args != null)
            {
                process.StartInfo.Arguments = args;
            }

            process.Start();

            if (redirectOutput)
            {
                process.WaitForExit();

                return new(process.ExitCode, process.StandardOutput.ReadToEnd(), process.StandardError.ReadToEnd());
            }
            else if (waitForExit)
            {
                process.WaitForExit();

                return new Result(process.ExitCode, null, null);
            }

            return null;
        }
    }

    public static class xPowershell
    {
        ///<summary>Execute PowerShell commands or entire scripts.</summary>
        public static String[] Run(String rawPSCommand, Boolean waitForExit = true, Boolean captureOutput = false)
        {
            if (waitForExit)
            {
                PowerShell.Create().AddScript(rawPSCommand).Invoke();

                return null;
            }
            else
            {
                ThreadStart e = new(() =>
                {
                    PowerShell.Create().AddScript(rawPSCommand).Invoke();
                });

                return null;
            }











            String[] LeProgg()
            {
                if (!captureOutput)
                {
                    PowerShell.Create().AddScript(rawPSCommand).Invoke();

                    return null;
                }

                List<String> PSOut = new();

                if (FormatCustom)
                {
                    rawPSCommand += " | Format-Custom";
                }

                foreach (var i in System.Management.Automation.PowerShell.Create().AddScript(rawPSCommand + " | Out-String").Invoke())
                {
                    if (i.ToString() is "")
                    {
                        continue;
                    }

                    PSOut.Add(i.ToString().Replace("\n", "").Replace("\r", ""));
                }

                for (Int32 i = 0; i < rmTop; i++)
                {
                    PSOut.RemoveAt(0);
                }

                for (Int32 i = 0; i < RMBottom; i++)
                {
                    PSOut.RemoveAt(PSOut.Count - 1);
                }

                return PSOut.ToArray();
            }
        }

        ///<summary>Tests if a PowerShell command can be executed.</summary>
        public static Boolean TestPSCommand(String RawPSCommand)
        {
            try
            {
                PowerShell.Create().AddCommand(RawPSCommand).Invoke();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void AAA()
        {
            PowerShell.Create().AddScript(rawPSCommand).Invoke();
        }









    }
}