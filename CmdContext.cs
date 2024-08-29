using System.Diagnostics;

namespace GitSeeker;

public class CmdContext
{
    private readonly Process Process;

    public void ExecuteCommand(string cmd)
    {
        Process.StandardInput.WriteLine(cmd);
    }

    public string Close()
    {
        var errors = Process.StandardError.ReadToEnd();
        var output = Process.StandardOutput.ReadToEnd();
        Process.WaitForExit();
        return output;
    }
    
    public CmdContext()
    {
        Process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = OperatingSystem.IsWindows()?"cmd.exe":"/bin/bash",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        Process.Start();
    }
}