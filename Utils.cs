using System.Text;
using GitSeeker.GitObjects;

namespace GitSeeker;

public static class Utils
{
    public static string ByteArrayToHex(byte[] array)
    {
        StringBuilder sb = new StringBuilder(array.Length*2);
        
        for (int i = 0; i < array.Length; i++)
        {
            sb.Append(array[i].ToString("x2"));
        }

        return sb.ToString();
    }

    public static string GenerateHashPath(string hash)
    {
        var h = hash.Replace("\n", "").Replace("\r", "").Trim();
        return h.Substring(0, 2) + "/" + hash.Substring(2, h.Length - 2);
    }

    public static (string path, string hash, bool directory)[] GetTreeRecursive(this IGitProvider provider, string rootHash)
    {
        List<(string path, string hash, bool directory)> objects = new();
        Queue<(string path, string tree)> queue = new();
        queue.Enqueue(("/", rootHash));
        while (queue.Count > 0)
        {
            var t = queue.Dequeue();
            var tree = provider.GetTree(t.tree);
            foreach (var obj in tree.Objects)
                if (obj.Directory)
                {
                    queue.Enqueue((t.path + obj.FileName + "/", obj.Hash));
                    objects.Add((t.path+obj.FileName, obj.Hash, true));
                }
                else
                    objects.Add((t.path+obj.FileName, obj.Hash, false));
        }

        return objects.ToArray();
    }
}