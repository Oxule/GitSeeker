using System.IO.Compression;

namespace GitSeeker;

public static class ZlibWrapper
{
    public static byte[] Decompress(byte[] data)
    {
        var ms = new MemoryStream(data);
        ms.Position = 0;
        var stream = new ZLibStream(ms, CompressionMode.Decompress);
        var ms2 = new MemoryStream();
        stream.CopyTo(ms2);
        return ms2.ToArray();
    }
}