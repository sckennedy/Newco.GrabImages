using System;
using System.IO;
using System.Net;

namespace GrabImages
{
    class Program
    {
        private const string baseUrl = "http://wwwin.cisco.com/dir/photo/zoom/";
        private static string imagesFolder = @"C:\Code\Images";
        private static string usernamesFile = @"C:\code\usernames.txt";
        
        static void Main(string [] args)
        {
            if (!string.IsNullOrEmpty(args[0]))
                imagesFolder = args[0];

            if (!string.IsNullOrEmpty(args[1]))
                usernamesFile = args[1];

            GetImages();
        }

        static void GetImages()
        {
            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            if(!File.Exists(usernamesFile))
            {
                Console.WriteLine($"{usernamesFile} does not exist.  Terminating.");
                Console.ReadLine();
                return;
            }

            string[] usernames = File.ReadAllLines(usernamesFile);

            foreach (string username in usernames)
            {
                Console.WriteLine($"Retrieving image for {username}");
                DownloadImage(username);
            }

            Console.WriteLine($"Images downloaded to {imagesFolder}");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static void DownloadImage(string username)
        {
            string fileName = $"{username}.jpg", myStringWebResource = null;
            try
            {
                WebClient myWebClient = new WebClient();
                myStringWebResource = baseUrl + fileName;
                myWebClient.DownloadFile(myStringWebResource, $"{imagesFolder}/{fileName}");
                Console.WriteLine("Done");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error getting {username} image.  Exception: {e.Message}.  {e.StackTrace}");
            }
        }
    }
}
   