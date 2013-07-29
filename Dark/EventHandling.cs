using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Dark.Archive.EventHandling
{
    public class Events
        {
            public virtual void OnFileParsed(ArgHandlers.OnFileParsed args)
            {

                EventHandler handler = FileParsed;
                if (handler != null)
                {
                    handler(this, args);
                }

            }
            public event EventHandler FileParsed;

            public virtual void OnErrorReceived(ArgHandlers.OnErrorReceived args)
            {

                EventHandler handler = ErrorReceived;
                if (handler != null)
                {
                    handler(this, args);
                }

            }
            public event EventHandler ErrorReceived;

        }
    namespace ArgHandlers
        {
            public class OnErrorReceived : EventArgs
            {
                public OnErrorReceived() { }
                public OnErrorReceived(Exception ex)
                {
                    this.ex = ex;
                }
                public Exception ex;
            }
            public class OnFileParsed : EventArgs
            {
                public bool Parsed { get; set; }
                public OnFileParsed() { }
                public OnFileParsed(bool parsed) { this.Parsed = parsed; }
            }
        }
}

