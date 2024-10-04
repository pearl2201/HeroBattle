using ICSharpCode.SharpZipLib.GZip;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Domain.Utils
{

    public class CompressUtility
    {
        public static string EncodeToBase64(byte[] data)
        {
            return System.Convert.ToBase64String(data);
        }

        public static byte[] DecodeFromBase64(string data)
        {
            return System.Convert.FromBase64String(data);
        }
        public static byte[] ZipText(string text)
        {
            if (text == null)
                return null;

            using (Stream memOutput = new MemoryStream())
            {
                using (GZipOutputStream zipOut = new GZipOutputStream(memOutput))
                {
                    using (StreamWriter writer = new StreamWriter(zipOut))
                    {
                        writer.Write(text);

                        writer.Flush();
                        zipOut.Finish();

                        byte[] bytes = new byte[memOutput.Length];
                        memOutput.Seek(0, SeekOrigin.Begin);
                        memOutput.Read(bytes, 0, bytes.Length);

                        return bytes;
                    }
                }
            }
        }

        public static string UnzipText(byte[] bytes)
        {
            if (bytes == null)
                return null;

            using (Stream memInput = new MemoryStream(bytes))
            using (GZipInputStream zipInput = new GZipInputStream(memInput))
            using (StreamReader reader = new StreamReader(zipInput))
            {
                string text = reader.ReadToEnd();

                return text;
            }
        }
    }

}
