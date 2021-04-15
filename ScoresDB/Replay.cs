using System;
using System.IO;
using System.Text;
using Decoder = SevenZip.Compression.LZMA.Decoder;

namespace ScoresDB
{
    public class Replay
    {
        public GameMode GameMode { get; private set; }

        public int Version { get; private set; }

        public string BeatmapMd5 { get; private set; }

        public string PlayerName { get; private set; }

        public string ReplayMd5 { get; private set; }

        public ushort Count300 { get; private set; }
        public ushort Count100 { get; private set; }
        public ushort Count50 { get; private set; }
        public ushort CountGeki { get; private set; }
        public ushort CountKatu { get; private set; }
        public ushort CountMiss { get; private set; }

        public int Score { get; private set; }
        public ushort Combo { get; private set; }
        public bool Perfect { get; private set; }

        public Mods Mods { get; private set; }

        public string LifeBarGraph { get; private set; }

        public long Timestamp { get; private set; }

        public int ReplayLength { get; private set; }

        private byte[] _replayData;

        public string ReplayData { get; private set; }

        public long ScoreId { get; private set; }

        public double? TpInfo { get; private set; }

        public bool IsScoreV2 => Mods.HasFlag(Mods.ScoreV2);

        public void ReadFromReader(OsuFileReader reader, bool parseReplay = false)
        {
            GameMode = (GameMode) reader.ReadByte();
            Version = reader.ReadInt32();
            BeatmapMd5 = reader.ReadString();
            PlayerName = reader.ReadString();
            ReplayMd5 = reader.ReadString();

            Count300 = reader.ReadUInt16();
            Count100 = reader.ReadUInt16();
            Count50 = reader.ReadUInt16();
            CountGeki = reader.ReadUInt16();
            CountKatu = reader.ReadUInt16();
            CountMiss = reader.ReadUInt16();

            Score = reader.ReadInt32();
            Combo = reader.ReadUInt16();
            Perfect = reader.ReadBoolean();
            Mods = (Mods) reader.ReadInt32();
            LifeBarGraph = reader.ReadString();
            Timestamp = reader.ReadInt64();
            ReplayLength = reader.ReadInt32();
            _replayData = reader.ReadBytes(ReplayLength);
            ScoreId = reader.ReadInt64();

            if (!parseReplay)
                return;

            var decoder = new Decoder();
            var replayReader = new BinaryReader(new MemoryStream(_replayData));
            var properties = replayReader.ReadBytes(5);
            var lengthBytes = replayReader.ReadBytes(8);
            var fileLength = BitConverter.ToInt64(lengthBytes, 0);

            var outStream = new MemoryStream();

            decoder.SetDecoderProperties(properties);
            decoder.Code(replayReader.BaseStream, outStream, replayReader.BaseStream.Length, fileLength, null);

            outStream.Seek(0, SeekOrigin.Begin);
            var buf = new byte[outStream.Length];
            outStream.Read(buf);

            ReplayData = Encoding.UTF8.GetString(buf);

            if (Mods.HasFlag(Mods.TargetPractice))
                TpInfo = reader.ReadDouble();
        }

        public override string ToString()
        {
            return $@"
Mode:         {GameMode}
osu! Version: {Version}
Beatmap:      {BeatmapMd5}
Player:       {PlayerName}
Replay Hash:  {ReplayMd5}
{Count300}x300 {Count100}x100 {Count50}x50 {CountGeki}xGeki {CountKatu}xKatu {CountMiss}xMiss {Combo}xCombo
Score:        {Score}
Perfect:      {Perfect}
Mods:         {Mods}
Graph:        {LifeBarGraph}
Timestamp:    {Timestamp}
Replay Size:  {ReplayLength}
Score ID:     {ScoreId}
TP Info: {TpInfo}
Replay Data:
{ReplayData}
";
        }
    }
}