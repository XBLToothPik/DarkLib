using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Dark.Methods;
namespace Dark.Archive.Structs
{
    public class TableEntry
    {
        public Int64 Offset { get; set; }
        public Int64 Unk1 { get; set; } //still don't know what this is
        public Int32 FileSize { get; set; }
        public Int32 NameSize { get; set; }
        public string Name { get; set; }

        public long writeoffset_offset;
        public Stream customStream { get; set; }
        public TableEntry(Stream xIn)
        {
            Offset = StreamUtils.ReadInt64(xIn, true);
            Unk1 = StreamUtils.ReadInt64(xIn, true);
            FileSize = StreamUtils.ReadInt32(xIn, true);
            NameSize = StreamUtils.ReadInt32(xIn, true);
            Name = new string(StreamUtils.ReadChars(xIn, NameSize));
        }
        public void Write(Stream xOut)
        {
            writeoffset_offset = xOut.Position;
            StreamUtils.WriteBytes(xOut, new byte[8]); //place holder for the offset
            StreamUtils.WriteInt64(xOut, Unk1, true);
            if (customStream != null)
                StreamUtils.WriteInt32(xOut, (Int32)customStream.Length, true);
            else
                StreamUtils.WriteInt32(xOut, FileSize, true);

            StreamUtils.WriteInt32(xOut, Name.Length, true);
            StreamUtils.WriteChars(xOut, Name.ToCharArray());
        }

        public void ExtractToStream(Stream xMain, Stream xOut)
        {
            if (customStream != null)
            {
                customStream.Position = 0;
                StreamUtils.ReadBufferedStream(customStream, (int)customStream.Length, xOut);
            }
            else
            {
                xMain.Position = Offset;
                StreamUtils.ReadBufferedStream(xMain, FileSize, xOut);
            }
        }

    }
}
