﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace GrabImages
{
    class Program
    {
        private const string BaseUrl = "http://wwwin.cisco.com/dir/photo/zoom/";
        private static string _imagesFolder = @"C:\Code\Images";
        private static string _usernamesFile = @"C:\code\usernames.txt";
        private static readonly List<string> NotFound = new List<string>();
        public static int AlreadyExistsCount;
        public static int NumberOfImagesRemoved;
        private static string[] _usernames;

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

            _usernames = File.ReadAllLines(_usernamesFile);

            foreach (string username in _usernames)
            {
                DownloadImage(username);
            }

            Console.WriteLine($"Images downloaded to {_imagesFolder}");
            if (NotFound.Count > 0)
            {
                var fileName = WriteOutputToFile(NotFound, "Images_Not_Found");
                Console.WriteLine($"Images not found  file created {fileName}");
            }

            RemoveUnnecessaryImages();

            Console.WriteLine($"{_usernames.Length} Usernames.");
            Console.WriteLine($"{AlreadyExistsCount} images already downloaded.");
            Console.WriteLine($"{_usernames.Length - AlreadyExistsCount} new downloads.");
            Console.WriteLine($"{NumberOfImagesRemoved} removed.");

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

        private static void RemoveUnnecessaryImages()
        {
            var d = new DirectoryInfo(_imagesFolder);
            var files = d.GetFiles("*.jpg");
            var filesToRemove = new List<FileInfo>();

            foreach (var file in files)
            {
                var filename = file.Name.Remove(file.Name.Length - 4);
                if (!_usernames.Contains(filename))
                {
                    filesToRemove.Add(file);
                }
            }

            NumberOfImagesRemoved = filesToRemove.Count;

            foreach (var fileInfo in filesToRemove)
            {
                fileInfo.Delete();
            }
        }
    }
}
   