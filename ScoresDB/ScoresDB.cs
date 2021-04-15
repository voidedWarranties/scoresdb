using System.Collections.Generic;
using System.Linq;

namespace ScoresDB
{
    public class ScoresDb
    {
        public readonly Dictionary<string, List<Replay>> Beatmaps = new();
        public int BeatmapCount => Beatmaps.Count;

        public ScoresDb Load(List<Replay> replays)
        {
            foreach (var replay in replays)
            {
                if (string.IsNullOrEmpty(replay.BeatmapMd5))
                    continue;

                if (!Beatmaps.ContainsKey(replay.BeatmapMd5))
                    Beatmaps[replay.BeatmapMd5] = new List<Replay>();

                Beatmaps[replay.BeatmapMd5].Add(replay);
            }

            foreach (var key in Beatmaps.Keys)
            {
                var scoresSorted = Beatmaps[key].Where(r => !r.IsScoreV2).OrderByDescending(r => r.Score);
                var scoreV2ScoresSorted = Beatmaps[key].Where(r => r.IsScoreV2)
                    .OrderByDescending(r => r.Score);

                Beatmaps[key] = scoresSorted.Concat(scoreV2ScoresSorted).ToList();
            }

            return this;
        }

        public void Write(OsuFileWriter writer)
        {
            const int version = 20210316;
            writer.Write(version);
            writer.Write(BeatmapCount);

            foreach (var (beatmapMd5, replays) in Beatmaps)
            {
                writer.Write(beatmapMd5);
                writer.Write(replays.Count);

                foreach (var score in replays)
                {
                    writer.Write((byte) score.GameMode);
                    writer.Write(score.Version);
                    writer.Write(score.BeatmapMd5);
                    writer.Write(score.PlayerName);
                    writer.Write(score.ReplayMd5);
                    writer.Write(score.Count300);
                    writer.Write(score.Count100);
                    writer.Write(score.Count50);
                    writer.Write(score.CountGeki);
                    writer.Write(score.CountKatu);
                    writer.Write(score.CountMiss);
                    writer.Write(score.Score);
                    writer.Write(score.Combo);
                    writer.Write(score.Perfect);
                    writer.Write((int) score.Mods);
                    writer.Write((string) null);
                    writer.Write(score.Timestamp);
                    writer.Write(-1);
                    writer.Write(score.ScoreId);

                    if (score.TpInfo.HasValue)
                        writer.Write(score.TpInfo.Value);
                }
            }
        }
    }
}