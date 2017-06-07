using System.IO;
using System.Diagnostics;

namespace TistoryConvertor
{
    public class Html2Markdown
    {
        public static string Convert(string content)
        {
            string temp_filename = Path.GetTempFileName();
            Util.WriteFileContent(temp_filename, content);

            string converted_filename = Path.GetTempFileName();
            string cmd = string.Format("pandoc -f html -t markdown_github {0} -o {1}",
                                       temp_filename, converted_filename);
            ExecuteCommandLine(cmd);

            string converted_content;
            using (var s = File.OpenText(converted_filename))
            {
                converted_content = s.ReadToEnd();

            }
            return converted_content;
        }

        private static void ExecuteCommandLine(string cmd)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + cmd;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
    }
}
