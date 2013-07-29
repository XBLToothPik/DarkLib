using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Dark.Methods;
using Dark.Archive.Structs;

namespace Dark.Archive
{
        public class DarkPack
        {

            public Dark.Archive.EventHandling.Events EventHandler { get; set; }
            public Header Header { get; set; }
            public Table Table { get; set; }
            private Stream xMain;

            public DarkPack() { EventHandler = new EventHandling.Events(); }
            public DarkPack(Stream xIn)
            {
                EventHandler = new EventHandling.Events();
                xMain = xIn;
                LoadFile();
            }

            public void OpenFile(Stream xIn)
            {
                xMain = xIn;
                LoadFile();
            }
            private void LoadFile()
            {
                Header = new Header(xMain, this.EventHandler);
                Table = new Table(xMain, Header);
                EventHandler.OnFileParsed(new EventHandling.ArgHandlers.OnFileParsed(true));
            }
            public void Save(Stream xOut)
            {
                Header.TableSize = 0;
                Header.FileCount = Table.Entries.Count;
                for (int i = 0; i < Table.Entries.Count; i++)
                    Header.TableSize += 24 + Table.Entries[i].NameSize;
                Header.Write(xOut);
                Table.Write(xOut, xMain);
            }

            public TableEntry GetEntryByName(string name)
            {
                for (int i = 0; i < Table.Entries.Count; i++)
                    if (Table.Entries[i].Name == name)
                        return Table.Entries[i];
                return null;
            }
        }
}
