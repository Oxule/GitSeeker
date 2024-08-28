using System.Text;

namespace GitSeeker.GitObjects;

public class GitTree
{
    public class GitTreeObject
    {
        public bool Directory;
        public string FileName;
        public string Hash;
    }

    public List<GitTreeObject> Objects;

    public GitTree(byte[] data)
    {
        Objects = new List<GitTreeObject>();
        int headerOffset = 0;
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] == '\0')
            {
                headerOffset = i+1;
                break;
            }
        }
        //<access>\s<name>\0<hash:20>
        GitTreeObject obj = new GitTreeObject();
        List<byte> buffer = new List<byte>(data.Length);
        byte state = 0;
        for (int i = headerOffset; i < data.Length; i++)
        {
            if (state == 0 && data[i] == ' ')
            {
                var access = int.Parse(Encoding.UTF8.GetString(buffer.ToArray()));
                obj.Directory = access == 40000;
                state = 1;
                buffer.Clear();
                continue;
            }
            if (state == 1 && data[i] == '\0')
            {
                obj.FileName = Encoding.UTF8.GetString(buffer.ToArray());
                state = 0;
                var hash = new byte[20];
                Array.Copy(data, i+1, hash, 0, 20);
                i += 20;
                obj.Hash = Utils.ByteArrayToHex(hash);
                Objects.Add(obj);
                obj = new GitTreeObject();
                buffer.Clear();
                continue;
            }
            buffer.Add(data[i]);
        }
    }
}