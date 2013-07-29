using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Dark.Methods;
namespace Dark.Archive.Structs
{
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
        public TableEntry GetEntryByName(string name)
        {
            for (int i = 0; i < this.Entries.Count; i++)
                if (this.Entries[i].Name == name)
                    return this.Entries[i];
            return null;
        }
    }
}
