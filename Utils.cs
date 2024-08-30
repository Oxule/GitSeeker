using System.Text;
using GitSeeker.GitObjects;
using YamlDotNet.Serialization;

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

    public static bool RepresentFocusDir(this string path, string focus)
    {
        var focusParts = new List<string>(focus.Split('\\', '/'));
        var pathParts = new List<string>(path.Split('\\', '/'));
        
        if(focusParts[^1] == "")
            focusParts.RemoveAt(focusParts.Count-1);
        if(pathParts[^1] == "")
            pathParts.RemoveAt(pathParts.Count-1);

        if (pathParts.Count < focusParts.Count)
            return false;
        
        for (int i = 0; i < focusParts.Count; i++)
        {
            if (pathParts[i] != focusParts[i])
                return false;
        }
        
        return true;
    }
    public static bool RepresentFocusDir(this string path, string focus, out string relativePath)
    {
        relativePath = "";
        var focusParts = new List<string>(focus.Split('\\', '/'));
        var pathParts = new List<string>(path.Split('\\', '/'));
        
        if(focusParts[^1] == "")
            focusParts.RemoveAt(focusParts.Count-1);
        if(pathParts[^1] == "")
            pathParts.RemoveAt(pathParts.Count-1);

        if (pathParts.Count < focusParts.Count)
            return false;
        
        for (int i = 0; i < focusParts.Count; i++)
        {
            if (pathParts[i] != focusParts[i])
                return false;
        }
        
        pathParts.RemoveRange(0, focusParts.Count);
        relativePath = '/'+string.Join('/', pathParts);
        
        return true;
    }

    
    public static bool TryGetParameter<T>(this Dictionary<string, object?> parameters, string name, out T? parameter)
    {
        parameter = default;
        
        if (parameters.TryGetValue(name, out var val) && val is string v)
        {
            try
            {
                var des = new Deserializer().Deserialize<T>(v);
                parameter = des;
                return true;
            }
            catch (Exception e) { }
        }
        
        return false;
    }
    public static void TryGetParameterChange<T>(this Dictionary<string, object?> parameters, string name, ref T? parameter)
    {
        if (parameters.TryGetParameter(name, out T? x))
        {
            parameter = x;
        }
    }
}