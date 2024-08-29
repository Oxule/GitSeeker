using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace GitSeeker;

public static class ActionVariables
{
    public static Regex VariableRegex = new (@"\$(\w+)", RegexOptions.Compiled);
    public static string EvaluateVariables(this string text, Dictionary<string, object?> variables)
    {
        return VariableRegex.Replace(text, match =>
        {
            string variableName = match.Groups[1].Value;
            
            if (variables.TryGetValue(variableName, out var value))
            {
                return JsonConvert.SerializeObject(value);
            }

            return match.Value;
        });
    }
}