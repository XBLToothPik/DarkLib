using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Dark.Methods
{
    public class StreamUtils
            {
                #region Reading
                public static byte ReadByte(Stream xIn)
                {
                    return (byte)xIn.ReadByte();
                }
                public static byte[] ReadBytes(Stream xIn, int len)
                {
                    byte[] buffer = new byte[len];
                    xIn.Read(buffer, 0, buffer.Length);
                    return buffer;
                }
                public static Int16 ReadInt16(Stream xIn, bool rev)
                {
                    byte[] buf = new byte[2];
                    xIn.Read(buf, 0, buf.Length);
                    if (rev)
                        Array.Reverse(buf);
                    return BitConverter.ToInt16(buf, 0);
                }
                public static Int32 ReadInt32(Stream xIn, bool rev)
                {
                    byte[] buf = new byte[4];
                    xIn.Read(buf, 0, buf.Length);
                    if (rev)
                        Array.Reverse(buf);
                    return BitConverter.ToInt32(buf, 0);
                }
                public static Int64 ReadInt64(Stream xIn, bool rev)
                {
                    byte[] buf = new byte[8];
                    xIn.Read(buf, 0, buf.Length);
                    if (rev)
                        Array.Reverse(buf);
                    return BitConverter.ToInt64(buf, 0);
                }
                public static char[] ReadNullChars(Stream xIn)
                {
                    string xBuilt = string.Empty;
                    while (true)
                    {
                        int read = xIn.ReadByte();
                        if (read == 0)
                            break;
                        xBuilt += Convert.ToChar(read);
                    }
                    return xBuilt.ToCharArray();
                }
                public static string ReadNullString(Stream xIn)
                {
                    string xBuilt = string.Empty;
                    while (true)
                    {
                        int read = xIn.ReadByte();
                        if (read == 0)
                            break;
                        xBuilt += Convert.ToChar(read);
                    }
                    return xBuilt;
                }
                public static char[] ReadChars(Stream xIn, int len)
                {
                    char[] xChars = new char[len];
                    for (int i = 0; i < len; i++)
                        xChars[i] = Convert.ToChar(xIn.ReadByte());
                    return xChars;
                }
                #endregion

                #region Writing
                public static void WriteByte(Stream xOut, byte val)
                {
                    xOut.Write(new byte[1] { val }, 0, 1);
                }
                public static void WriteBytes(Stream xOut, byte[] buffer)
                {
                    xOut.Write(buffer, 0, buffer.Length);
                }
                public static void WriteInt16(Stream xOut, Int16 val, bool rev)
                {
                    byte[] buffer = BitConverter.GetBytes(val);
                    if (rev)
                        Array.Reverse(buffer);
                    xOut.Write(buffer, 0, buffer.Length);
                }
                public static void WriteInt32(Stream xOut, Int32 val, bool rev)
                {
                    byte[] buffer = BitConverter.GetBytes(val);
                    if (rev)
                        Array.Reverse(buffer);
                    xOut.Write(buffer, 0, buffer.Length);
                }
                public static void WriteInt64(Stream xOut, Int64 val, bool rev)
                {
                    byte[] buffer = BitConverter.GetBytes(val);
                    if (rev)
                        Array.Reverse(buffer);
                    xOut.Write(buffer, 0, buffer.Length);
                }
                public static void WriteNullChars(Stream xOut, char[] chars)
                {
                    byte[] buffer = new byte[chars.Length];
                    for (int i = 0; i < buffer.Length; i++)
                        buffer[i] = Convert.ToByte(chars[i]);
                    xOut.Write(buffer, 0, buffer.Length);
                    xOut.Write(new byte[1] { 0x0 }, 0, 1);
                }
                public static void WriteNullString(Stream xOut, string str)
                {
                    byte[] buffer = new byte[str.Length];
                    for (int i = 0; i < buffer.Length; i++)
                        buffer[i] = Convert.ToByte(str[i]);
                    xOut.Write(buffer, 0, buffer.Length);
                    xOut.Write(new byte[1] { 0x0 }, 0, 1);
                }
                public static void WriteChars(Stream xOut, char[] chars)
                {
                    byte[] buffer = new byte[chars.Length];
                    for (int i = 0; i < buffer.Length; i++)
                        buffer[i] = Convert.ToByte(chars[i]);
                    xOut.Write(buffer, 0, buffer.Length);
                }
                #endregion

                #region Other

                /// <summary>
                /// Reads bytes out of one stream into another, does so with buffers for good memory use
                /// </summary>
                /// <param name="xMain">Main stream to read out of</param>
                /// <param name="readLen">Length to read</param>
                /// <param name="xOut">Stream to read ito</param>
                public static void ReadBufferedStream(Stream xMain, int readLen, Stream xOut)
                {
                    int toOffset = (int)xMain.Position + readLen;
                    byte[] buffer;
                    while (xMain.Position < toOffset)
                    {
                        int rLength = ((toOffset - xMain.Position) >= 65536) ? 65536 : (toOffset - (int)xMain.Position);
                        buffer = new byte[rLength];
                        xMain.Read(buffer, 0, buffer.Length);
                        xOut.Write(buffer, 0, buffer.Length);
                    }
                }
                #endregion
            }
    public static class StringUtils
    {
        public static bool IsNullOrWhiteSpace(string value)
        {
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (!char.IsWhiteSpace(value[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
