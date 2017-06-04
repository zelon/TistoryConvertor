using System.IO;

namespace TistoryConvertor
{
    class Util
    {
        public static void WriteFileContent(string filename, string content)
        {
            byte[] encoded_bytes = System.Text.Encoding.UTF8.GetBytes(content);
            using (var file_stream = File.CreateText(filename))
            {
                file_stream.Write(content);
                //file_stream.Write(encoded_bytes, 0, encoded_bytes.Length);
            }
        }
    }
}
