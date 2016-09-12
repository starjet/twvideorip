using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string main_m3u8 = Environment.GetCommandLineArgs()[1];
            main_m3u8 = main_m3u8.Replace(@"\/", @"/");
            new WebClient().DownloadFile(main_m3u8, "main.m3u8");
            string[] lines = File.ReadAllLines("main.m3u8");
            List<string> urls = new List<string>();
            foreach (string line in lines)
            {
                if (line.Contains(@"/ext_tw_video/"))
                {
                    urls.Add(line.Replace(@"/ext_tw_video/", @"https://video.twimg.com/ext_tw_video/"));
                }
            }
            string highest_m3u8 = urls.Last();
            new WebClient().DownloadFile(highest_m3u8, "highest.m3u8");
            lines = File.ReadAllLines("highest.m3u8");
            List<string> fixedLines = new List<string>();
            foreach (string line in lines)
            {
                fixedLines.Add(line.Replace(@"/ext_tw_video/", @"https://video.twimg.com/ext_tw_video/"));
            }
            File.WriteAllLines("highest.m3u8", fixedLines);
            Process ffmpeg = new Process();
            ffmpeg.StartInfo.FileName = "ffmpeg.exe";
            ffmpeg.StartInfo.Arguments = "-i highest.m3u8 -acodec copy -vcodec copy " + main_m3u8.Split('/').Last().Replace(".m3u8", "") + ".ts";
            ffmpeg.Start();
            ffmpeg.WaitForExit();
        }
    }
}
