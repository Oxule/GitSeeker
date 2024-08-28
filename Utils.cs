using System.Text;

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
}