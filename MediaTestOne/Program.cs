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

        // Check if file exists
        if(!File.Exists(path))
        {
            // If not, create it
            using(var myFile = File.Create(path))
            {
                // Do nothing with the file but create it
            }
        }

        while (true)
        {
            // Don't check every clock cycle, do some sleeping
            Thread.Sleep(400);

            // Await the tasks below
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