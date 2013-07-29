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
    
}