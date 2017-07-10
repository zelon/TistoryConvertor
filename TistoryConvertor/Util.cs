using System.IO;

namespace TistoryConvertor
{
    class Util
    {
        public static void WriteFileContent(string filename, string content)
        {
            using (var file_stream = File.CreateText(filename))
            {
                file_stream.Write(content);
            }
        }
    }
}
