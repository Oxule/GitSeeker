namespace GitSeeker;

public static class ActionRunner
{
    public class Action
    {
        public string Name;
        public delegate void ActionMethod(Dictionary<string, object?> parameters, IGitProvider provider, Configuration.Service service, Dictionary<string, object?> variables);
        public ActionMethod Method;

        public Action(string name, ActionMethod method)
        {
            Name = name;
            Method = method;
        }
    }

    private static List<Action> Actions = new();

    public static void AddAction(Action action) => Actions.Add(action);
    public static void AddAction(string name, Action.ActionMethod method) => Actions.Add(new (name,method));

    public static void StartAction(string name, Dictionary<string, object?> parameters, IGitProvider provider,
        Configuration.Service service, Dictionary<string, object?> variables)
    {
        foreach (var a in Actions)
        {
            if (a.Name == name)
            {
                a.Method.Invoke(parameters, provider, service, variables);
                return;
            }
        }
        Console.WriteLine($"Didn't found action type {name}");
    }
}