using System.Diagnostics;

namespace GitSeeker;

public static class DefaultActions
{
    public static void AddDefaultActions()
    {
        AddCmdInterpreter();
        AddGitClone();
    }

    private static void AddCmdInterpreter()
    {
        ActionRunner.AddAction("cmd", (parameters, _, _, variables) =>
        {
            var context = new CmdContext();
            
            if (parameters.TryGetValue("cmd", out var cmd))
            {
                if (cmd is List<object> cmdList)
                {
                    foreach (var c in cmdList)
                        if (c is string cs)
                            context.ExecuteCommand(cs.EvaluateVariables(variables));
                }
                else if(cmd is string c)
                    context.ExecuteCommand(c.EvaluateVariables(variables));
            }
            context.ExecuteCommand("exit");
            Console.WriteLine(context.Close());
        });
    }

    private static void AddGitClone()
    {
        ActionRunner.AddAction("clone", (parameters, provider, service, variables) =>
        {
            bool focused = true;
            parameters.TryGetParameterChange("focused", ref focused);
            
            var focus = focused ? service.Repository.Focus : "/";
            
            bool clear = true;
            parameters.TryGetParameterChange("clear", ref clear);
            
            if (!parameters.TryGetParameter<string>("path", out var path) || path == null)
            {
                Console.WriteLine($"No clone path provided!");
                return;
            }
            path = path.EvaluateVariables(variables);
            
            var branch = provider.GetBranches().FirstOrDefault(b => b.Name == service.Repository.Branch);
            if (branch == null)
                return;

            var commit = provider.GetCommit(branch.LastCommit);
            var files = provider.GetTreeRecursive(commit.Tree);

            if (clear)
            {
                foreach (var f in Directory.GetFiles(path))
                    File.Delete(f);
                foreach (var d in Directory.GetDirectories(path))
                    Directory.Delete(d, true);
            }
            
            foreach (var f in files.OrderByDescending(x=>x.directory?1:0))
            {
                if(!f.path.RepresentFocusDir(focus, out var rel))
                    continue;
                var target = path + rel;
                Console.WriteLine($"Copying {f.path} -> {target}");
                if (f.directory)
                    Directory.CreateDirectory(target);
                else
                    File.WriteAllBytes(target, provider.GetBlob(f.hash).Data);
            }
        });
    }
}