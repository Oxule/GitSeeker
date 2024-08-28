using System.Text;
using GitSeeker.GitObjects;

namespace GitSeeker;

public class LocalGitProvider : IGitProvider
{
    public string Path;

    public LocalGitProvider(string path)
    {
        Path = path;
    }

    public GitTree GetTree(string hash)
    {
        var path = Path + "/objects/" + Utils.GenerateHashPath(hash);
        var file = File.ReadAllBytes(path);
        var data = ZlibWrapper.Decompress(file);
        return new GitTree(data);
    }
    public GitCommit GetCommit(string hash)
    {
        var path = Path + "/objects/" + Utils.GenerateHashPath(hash);
        var file = File.ReadAllBytes(path);
        var data = Encoding.UTF8.GetString(ZlibWrapper.Decompress(file));
        return new GitCommit(data);
    }
    public GitBlob GetBlob(string hash)
    {
        var path = Path + "/objects/" + Utils.GenerateHashPath(hash);
        var file = File.ReadAllBytes(path);
        var data = ZlibWrapper.Decompress(file);
        return new GitBlob(data);
    }

    public GitBranch[] GetBranches()
    {
        List<GitBranch> branches = new ();
        foreach (var head in Directory.GetFiles(Path + "/refs/heads/"))
        {
            branches.Add(new GitBranch(
                System.IO.Path.GetFileNameWithoutExtension(head), 
                File.ReadAllText(head)));
        }
        return branches.ToArray();
    }

    public GitTag[] GetTags()
    {
        return [];
    }
}