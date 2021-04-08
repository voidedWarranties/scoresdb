using System;
using System.IO;
using System.Text;

namespace ScoresDB
{
    public class OsuFileReader : BinaryReader
    {
        public OsuFileReader(Stream input) : base(input)
        {
        }

        public OsuFileReader(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        public OsuFileReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        public override string ReadString()
        {
            switch (ReadByte())
            {
                case 0x00:
                    return null;
                case 0x0b:
                    return base.ReadString();
                default:
                    throw new Exception("Invalid string type byte");
            }
        }
    }
}