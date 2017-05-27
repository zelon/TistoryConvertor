using System.IO;
using System.Text.RegularExpressions;

namespace TistoryConvertor
{
    class MarkdownConvertor
    {
        public static string ToMarkdown(Post post)
        {
            string converted = ReplaceImagePart(post.Content);
            converted = ConvertByPandoc(converted);
            return converted;
        }

        private static string ReplaceImagePart(string content)
        {
            Regex assert_ex = new Regex(@"\[##.*?##\]");
            var match_collection = assert_ex.Matches(content);
            foreach (var match in match_collection)
            {
                // ##_1C로 시작해서, |_##] 로 항상 끝나는지 체크한다
                System.Diagnostics.Debug.Assert(match.ToString().StartsWith("[##_1C|"));
                System.Diagnostics.Debug.Assert(match.ToString().EndsWith("|_##]"));
            }
            Regex ex = new Regex("\\[##_1C.*?width=\\\"(\\d+)\\\" height=\\\"(\\d+)\\\" filename=\\\"(.*?)\\\".*?##\\]");
            match_collection = ex.Matches(content);
            foreach (Match match in match_collection)
            {
                string width = match.Groups[1].Value;
                string height = match.Groups[2].Value;
                string filename = match.Groups[3].Value;
            }
            
            string replaced = ex.Replace(content, "<img src=\"$3\" width=\"$1\" height=\"$2\" />");
            return replaced;
        }

        private static string ConvertByPandoc(string content)
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
