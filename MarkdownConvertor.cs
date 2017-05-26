using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace TistoryConvertor
{
    class MarkdownConvertor
    {
        public static string ToMarkdown(string content)
        {
            content = ReplaceImagePart(content);
            content = ConvertByPandoc(content);
            return content;
        }

        private static string ReplaceImagePart(string content)
        {
            return content;
        }
        private static string ConvertByPandoc(string content)
        {
            // pandoc -f html -t markdown_github test.html > test.md
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
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + cmd;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

    }
}
