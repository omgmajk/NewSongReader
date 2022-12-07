using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Control;

public static class Program
{
    public static async Task Main(string[] args)
    {
        // Storage vars
        string path = @".\Song.txt";

        if(!File.Exists(path))
        {
            using(var myFile = File.Create(path))
            {

            }
        }

        while (true)
        {
            // Just a microsleep
            Thread.Sleep(400);

            // Await the methods
            var gsmtcsm = await GetSystemMediaTransportControlsSessionManager();
            var mediaProperties = await GetMediaProperties(gsmtcsm.GetCurrentSession());
            
            // Get the song title
            var songTitle = String.Format("{0} - {1}", mediaProperties.Artist, mediaProperties.Title);

            // Read the written text file
            string textFile = System.IO.File.ReadAllText(path);

            // Debug shit
            //Console.WriteLine($"From file: \"{textFile.Trim()}\"\nFrom await: \"{songTitle.Trim()}\"\n");

            if (textFile.Trim() == songTitle.Trim())
            {
                continue; // Don't write if we already wrote the filename
            }

            // Write the Song.txt-file
            try
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    Console.WriteLine($"Writing {songTitle} to textfile");
                    sw.WriteLine(songTitle);
                }
            } 
            catch
            {
                Console.WriteLine("\n\nFile not written, something went wrong...");
                Console.WriteLine("\n\nPress any key to close the application.");
                break;
            }
        }

        Console.ReadKey();
    }

    // Async tasks to check the media transfer thing
    private static async Task<GlobalSystemMediaTransportControlsSessionManager> GetSystemMediaTransportControlsSessionManager() =>
        await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();

    private static async Task<GlobalSystemMediaTransportControlsSessionMediaProperties> GetMediaProperties(GlobalSystemMediaTransportControlsSession session) =>
        await session.TryGetMediaPropertiesAsync();
}