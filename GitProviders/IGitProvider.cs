using GitSeeker.GitObjects;

namespace GitSeeker;

public interface IGitProvider
{
    public GitTree GetTree(string hash);
    public GitCommit GetCommit(string hash);
    public GitBlob GetBlob(string hash);
    public GitBranch[] GetBranches();
    public GitTag[] GetTags();
}