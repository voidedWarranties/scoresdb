using System.IO;

namespace ScoresDB
{
    public class OsuFileWriter : BinaryWriter
    {
        public OsuFileWriter(Stream stream)
            : base(stream)
        {
        }

        public override void Write(string value)
        {
            if (value == null)
                Write((byte) 0x00);
            else
            {
                Write((byte) 0x0b);
                base.Write(value);
            }
        }
    }
}