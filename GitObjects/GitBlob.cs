namespace GitSeeker.GitObjects;

public class GitBlob
{
    public byte[] Data;

    public GitBlob(byte[] data)
    {
        int startIndex = 0;
        byte spaceCount = 0;
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] == ' ')
            {
                spaceCount++;
                if (spaceCount == 2)
                    startIndex = i + 1;
            }
        }

        Data = new byte[data.Length - startIndex];
        Array.Copy(data, startIndex, Data, 0, data.Length - startIndex);
    }
}