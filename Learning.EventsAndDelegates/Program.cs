using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

    class Program
    {
        static void Main(string[] args)
        {
            var video = new Video() {Title = "Video 1"};
            var videoEncoder = new VideoEncoder(); // publisher
            var mailService = new MailService(); // subscriber
            var messageService = new MessageService();

            videoEncoder.VideoEncoded += mailService.OnVideoEncoded;
            videoEncoder.VideoEncoded += messageService.OnVideoEncoded;

            videoEncoder.Encode(video);

        }
    }
}
