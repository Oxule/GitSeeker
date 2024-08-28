namespace GitSeeker.GitObjects;

public class GitBranch
{
    public string Name;
    public string LastCommit;

    public GitBranch(string name, string lastCommit)
    {
        Name = name;
        LastCommit = lastCommit;
    }
}