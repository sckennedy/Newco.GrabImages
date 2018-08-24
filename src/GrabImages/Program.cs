using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace GrabImages
{
    class Program
    {
        private const string baseUrl = "http://wwwin.cisco.com/dir/photo/zoom/";
        private static string imagesFolder = @"C:\Code\Images";
        private static string usernamesFile = @"C:\code\usernames.txt";
        private static List<string> notFound = new List<string>();
        
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
                DownloadImage(username);
            }

            Console.WriteLine($"Images downloaded to {imagesFolder}");
            if (notFound.Count > 0)
            {
                var fileName = WriteOutputToFile(notFound, "Images_Not_Found");
                Console.WriteLine($"Images not found  file created {fileName}");
            }
            
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static string WriteOutputToFile(List<string> lines, string name)
        {
            //create filename based on timestamp of the day
            var fileName = $@"{imagesFolder}\{name}_{DateTime.Now.ToShortDateString().Replace("/", "_")}-{DateTime.Now.ToShortTimeString().Replace(":", "-")}.txt";
            File.WriteAllLines(fileName, lines.ToArray());
            return fileName;
        }

        private static void DownloadImage(string username)
        {
            string fileName = $"{username}.jpg", myStringWebResource = null;
            try
            {
                WebClient myWebClient = new WebClient();
                myStringWebResource = baseUrl + fileName;
                myWebClient.DownloadFile(myStringWebResource, $"{imagesFolder}/{fileName}");
                Console.WriteLine($"{username} done");
            }
            catch(Exception e)
            {
                notFound.Add(username);
                Console.WriteLine($"Error getting {username} image.  Exception: {e.Message}.  {e.StackTrace}");
            }
        }
    }
}
   