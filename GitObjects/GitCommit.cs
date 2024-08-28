using System.Text;

namespace GitSeeker.GitObjects;

public class GitCommit
{
    public int Size;
    public string Tree;
    public string? Parent;
    public string Author;
    public string Committer;
    public string Message;

    public GitCommit(string message)
    {
        var lines = message.Split('\n', '\0');
        int messageLine = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] == "")
            {
                messageLine = i + 1;
                break;
            }
            var parts = lines[i].Split(' ');
            switch (parts[0])
            {
                case "commit":
                    Size = int.Parse(parts[1]);
                    break;
                case "tree":
                    Tree = parts[1];
                    break;
                case "parent":
                    Parent = parts[1];
                    break;
                case "author":
                    Author = string.Join(' ', parts, 1, parts.Length - 1);
                    break;
                case "committer":
                    Committer = string.Join(' ', parts, 1, parts.Length - 1);
                    break;
            }
        }

        StringBuilder sb = new StringBuilder();
        for (int i = messageLine; i < lines.Length; i++)
        {
            sb.AppendLine(lines[i]);
        }

        Message = sb.ToString();
    }

    private string RemoveKey(string line, string key) => line.Substring(key.Length, line.Length - key.Length);
}