using System.Security.Cryptography;
using System.Text;

namespace GitSeeker;

public class RepoState
{
    public string LastCommit;
    public Dictionary<string, string> TreeHashes;

    public static string CalcRepoHash(string name, string path, string branch)
    {
        var str = string.Join('_', name, path, branch);
        var hash = SHA1.HashData(Encoding.UTF8.GetBytes(str));
        return Utils.ByteArrayToHex(hash);
    }
    
    public RepoState(string lastCommit, Dictionary<string, string> treeHashes)
    {
        LastCommit = lastCommit;
        TreeHashes = treeHashes;
    }
}