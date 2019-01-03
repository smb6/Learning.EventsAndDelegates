using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Learning.EventsAndDelegates
{
    public class Video
    {
        public string Title { get; set; }
    }

    public class VideoEventArgs : EventArgs
    {
        public Video Video { get; set; }
    }
    public class VideoEncoder
    {
        // 1 - Define a delegate. 
        // 2 - Define an event based on delegate
        // 3 - Raise the event

        // 1.
//        public delegate void VideoEncodedEventHandler(object source, EventArgs args);
//        public delegate void VideoEncodedEventHandler(object source, VideoEventArgs args);
        // EventHandler
        // EventHandler<TEventsArgs>
        // instead of defining a delegate and an event
        public event EventHandler<VideoEventArgs> VideoEncoded;
        // 2.
//        public event VideoEncodedEventHandler VideoEncoded;
        
        // 3.
//        protected virtual void OnVideoEncoded()
        protected virtual void OnVideoEncoded(Video video)
        {
            // check if any subscribers to event
            if (VideoEncoded != null)
            {
//                VideoEncoded(this, EventArgs.Empty);
                VideoEncoded(this, new VideoEventArgs(){Video = video });
            }
        }

        public void Encode(Video video)
        {
            Console.WriteLine("Encoding video...");
            Thread.Sleep(3000);
//            OnVideoEncoded();
            OnVideoEncoded(video);
        }
    }

    public class MailService
    {
//        public void OnVideoEncoded(object source, EventArgs e)
        public void OnVideoEncoded(object source, VideoEventArgs e)
        {
            Console.WriteLine("MailService: sending an email..." + e.Video.Title);
        }
    }

    public class MessageService
    {
//        public void OnVideoEncoded(object source, EventArgs e)
        public void OnVideoEncoded(object source, VideoEventArgs e)
        {
            Console.WriteLine("MessageService: sending a text..." + e.Video.Title);
        }
    }

    public class SendXmlFile
    {
        public void OnVideoEncoded(Object source, VideoEventArgs e)
        {
            Console.WriteLine($"SendXmlFile: Writting XML file to.. {1}", e.Video.Title);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //var video = new Video() {Title = "Video 1"};
            //var videoEncoder = new VideoEncoder(); // publisher
            //var mailService = new MailService(); // subscriber
            //var xmlFileService = new SendXmlFile();
            //var messageService = new MessageService();

            //videoEncoder.VideoEncoded += mailService.OnVideoEncoded;
            //videoEncoder.VideoEncoded += messageService.OnVideoEncoded;
            //videoEncoder.VideoEncoded += xmlFileService.OnVideoEncoded;

            //videoEncoder.Encode(video);

            IEnumerable<string> files = GetFiles("*.txt", @"D:\TMP\tmp1", @"D:\TMP\tmp2", @"D:\TMP\tmp3");
            string[] directories = { @"D:\TMP\tmp1", @"D:\TMP\tmp2", @"D:\TMP\tmp3" };

            foreach(var directory in directories)
            {
                foreach (var file in Directory.GetFiles(directory, "*.txt"))
                {
                    ReadFile(file);
                }
            }

            FileSystemWatcher watcher = new FileSystemWatcher()
            {
                Path = $"C:\\TMP",
                Filter = "*.txt"
            };
            // Add event handlers for all events you want to handle
            watcher.Created += new FileSystemEventHandler(OnChanged);
            // Activate the watcher
            watcher.EnableRaisingEvents = true;
            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            System.Diagnostics.Debug.Write($"File: {e.FullPath} was added");

            var contents = File.ReadAllText(e.FullPath);
            Console.WriteLine($"File {e.FullPath} content is {contents}");
            Thread.Sleep(400);
            File.Delete(e.FullPath);
        }

        private static void ReadFile(string filePath)
        {
            var content = File.ReadAllText(filePath);
            Console.WriteLine($"File {filePath} with content {content}");
        }

        private static IEnumerable<string> GetFiles(string searchPattern, params string[] directories)
        {
            foreach (string directory in directories)
            {
                foreach (string file in Directory.GetFiles(directory, searchPattern))
                    yield return file;
            }
        }

    }
}
