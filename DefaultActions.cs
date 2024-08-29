using System.Diagnostics;

namespace GitSeeker;

public static class DefaultActions
{
    public static void AddDefaultActions()
    {
        AddCmdInterpreter();
    }

    private static void AddCmdInterpreter()
    {
        ActionRunner.AddAction("cmd", (parameters, _, _, variables) =>
        {
            var context = new CmdContext();
            
            if (parameters.ContainsKey("cmd"))
            {
                var c = parameters["cmd"]!;
                if (c.GetType() == typeof(List<object>))
                {
                    foreach (var cmd in (List<object>)c)
                        if (cmd is string)
                            context.ExecuteCommand(((string)cmd).EvaluateVariables(variables));
                }
                else if(c is string)
                    context.ExecuteCommand(((string)c).EvaluateVariables(variables));
            }
            context.ExecuteCommand("exit");
            Console.WriteLine(context.Close());
        });
    }
}