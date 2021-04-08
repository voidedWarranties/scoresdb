using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;

// reference https://github.com/HoLLy-HaCKeR/osu-database-reader/
namespace ScoresDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var directory = Path.Combine(GetStableInstallPathFromRegistry(), @"Data\r");

            var replays = new List<Replay>();

            foreach (string file in Directory.EnumerateFiles(directory, "*.osr"))
            {
                var replay = new Replay();
                replay.ReadFromReader(new OsuFileReader(File.OpenRead(file)));
                replays.Add(replay);
            }

            var db = new ScoresDb();
            db.Load(replays).Write(new OsuFileWriter(File.OpenWrite("scores.db")));
        }

        // https://github.com/ppy/osu/blob/ef1f133fb1a65b69df1a29beb72707d8f897e8a4/osu.Desktop/OsuGameDesktop.cs#L86-L91
        private static string GetStableInstallPathFromRegistry()
        {
            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey("osu"))
                return key?.OpenSubKey(@"shell\open\command")?.GetValue(string.Empty)?.ToString()?.Split('"')[1]
                    .Replace("osu!.exe", "");
        }
    }
}