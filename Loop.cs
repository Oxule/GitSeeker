namespace GitSeeker;

public static class Seeker
{
    public const int SEEK_DELAY_MS = 5000;
    
    public static void Loop(Configuration config)
    {
        while (true)
        {
            foreach (var service in config.Services)
            {
                Console.WriteLine($"Checking [{service.Name}]");
            }
            Thread.Sleep(SEEK_DELAY_MS);
        }
    }
}