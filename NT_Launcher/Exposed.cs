using System;
using System.Diagnostics;
using System.Linq;
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
                String stdOUT = process.StandardOutput.ReadToEnd();
                String errOUT = process.StandardError.ReadToEnd();

                process.WaitForExit();

                return new(process.ExitCode, stdOUT, errOUT);
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
            if (captureOutput)
            {
                PSObject psObject = PowerShell.Create().AddScript(rawPSCommand + " | Out-String").Invoke().First();

                return psObject.ToString().Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (waitForExit)
            {
                PowerShell.Create().AddScript(rawPSCommand).Invoke();
            }
            else
            {
                ThreadStart commandWorker = new(() =>
                {
                    PowerShell.Create().AddScript(rawPSCommand).Invoke();
                });
            }

            return null;
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
    }
}