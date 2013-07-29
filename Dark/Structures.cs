using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Dark.Archive.EventHandling;
using Dark.Archive.EventHandling.ArgHandlers;
using Dark.Methods;

namespace Dark.Archive.Structs
{
    public class Header
    {
        public Int32 TableSize { get; set; }
        public Int32 FileCount { get; set; }
        public Header(Stream xIn, Events EventHandler)
        {
            if (!(new string(StreamUtils.ReadChars(xIn, 8)) == "DARKPACK"))
                EventHandler.OnErrorReceived(new OnErrorReceived(new InvalidDataException("Invalid File Header")));
            TableSize = StreamUtils.ReadInt32(xIn, true);
            FileCount = StreamUtils.ReadInt32(xIn, true);
        }
        public void Write(Stream xOut)
        {
            StreamUtils.WriteChars(xOut, "DARKPACK".ToCharArray());
            StreamUtils.WriteInt32(xOut, TableSize, true);
            StreamUtils.WriteInt32(xOut, FileCount, true);
        }
    }
    public class Table
    {
        public List<TableEntry> Entries { get; set; }
        public Table(Stream xIn, Header hdr)
        {
            this.Entries = new List<TableEntry>();
            for (int i = 0; i < hdr.FileCount; i++)
                Entries.Add(new TableEntry(xIn));
        }
        public void Write(Stream xOut, Stream xMain)
        {
            for (int i = 0; i < Entries.Count; i++)
                Entries[i].Write(xOut);

            for (int i = 0; i < Entries.Count; i++)
            {
                long xPos = xOut.Position;
                xOut.Position = Entries[i].writeoffset_offset;
                StreamUtils.WriteInt64(xOut, xPos, true);
                xOut.Position = xPos;

                if (Entries[i].customStream != null)
                {
                    Entries[i].customStream.Seek(0, SeekOrigin.Begin);
                    StreamUtils.ReadBufferedStream(Entries[i].customStream, (int)Entries[i].customStream.Length, xOut);
                }
                else
                {
                    xMain.Position = Entries[i].Offset;
                    StreamUtils.ReadBufferedStream(xMain, Entries[i].FileSize, xOut);
                }
            }
        }
    }
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