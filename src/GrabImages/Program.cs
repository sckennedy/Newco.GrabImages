using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace GrabImages
{
    class Program
    {
        private const string BaseUrl = "http://wwwin.cisco.com/dir/photo/zoom/";
        private static string _imagesFolder = @"C:\Code\Images";
        private static string _usernamesFile = @"C:\code\usernames.txt";
        private static readonly List<string> NotFound = new List<string>();
        public static int AlreadyExistsCount = 0;
        
        static void Main(string [] args)
        {
            if (!string.IsNullOrEmpty(args[0]))
                _imagesFolder = args[0];

            if (!string.IsNullOrEmpty(args[1]))
                _usernamesFile = args[1];

            GetImages();
        }

        static void GetImages()
        {
            if (!Directory.Exists(_imagesFolder))
                Directory.CreateDirectory(_imagesFolder);

            if(!File.Exists(_usernamesFile))
            {
                Console.WriteLine($"{_usernamesFile} does not exist.  Terminating.");
                Console.ReadLine();
                return;
            }

            string[] usernames = File.ReadAllLines(_usernamesFile);

            foreach (string username in usernames)
            {
                DownloadImage(username);
            }

            Console.WriteLine($"Images downloaded to {_imagesFolder}");
            if (NotFound.Count > 0)
            {
                var fileName = WriteOutputToFile(NotFound, "Images_Not_Found");
                Console.WriteLine($"Images not found  file created {fileName}");
            }

            Console.WriteLine($"{usernames.Length} Usernames.");
            Console.WriteLine($"{AlreadyExistsCount} images already downloaded.");
            Console.WriteLine($"{usernames.Length - AlreadyExistsCount} new downloads.");

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static string WriteOutputToFile(List<string> lines, string name)
        {
            //create filename based on timestamp of the day
            var fileName = $@"{_imagesFolder}\{name}_{DateTime.Now.ToShortDateString().Replace("/", "_")}-{DateTime.Now.ToShortTimeString().Replace(":", "-")}.txt";
            File.WriteAllLines(fileName, lines.ToArray());
            return fileName;
        }

        private static void DownloadImage(string username)
        {
            var fileName = $"{username}.jpg";

            if (!ImageExists(username))
            {
                var myWebClient = new WebClient();
                try
                {
                    var myStringWebResource = BaseUrl + fileName;
                    myWebClient.DownloadFile(myStringWebResource, $"{_imagesFolder}/{fileName}");
                    Console.WriteLine($"{username} done");
                }
                catch (Exception e)
                {
                    NotFound.Add(username);
                    Console.WriteLine($"Error getting {username} image.  Exception: {e.Message}.  {e.StackTrace}");
                }
            }
            else
            {
                AlreadyExistsCount++;
            }
        }

        private static bool ImageExists(string username)
        {
            return File.Exists($"{_imagesFolder}/{username}.jpg");
        }
    }
}
   