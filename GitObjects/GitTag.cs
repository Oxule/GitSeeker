namespace GitSeeker.GitObjects;

public class GitTag
{
    public string Tag;
    public string Commit;

    public GitTag(string tag, string commit)
    {
        Tag = tag;
        Commit = commit;
    }
}