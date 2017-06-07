using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace TistoryConvertor
{
    public class MarkdownConvertor
    {
        // width, height에는 소수점이 있을 때도 있어서 \d 를 사용할 수 없다

        public static string ToMarkdown(Post post)
        {
            AssertAllImageReplacerMark(post);
            string converted = ReplaceImagePart(post);
            AssertReferencingImageFileExsit(post, converted);
            converted = ConvertByPandoc(converted);
            return converted;
        }

        // 모든 이미지 태그가 ##_1C로 시작해서, |_##] 로 항상 끝나는지 체크한다
        private static void AssertAllImageReplacerMark(Post post)
        {
            Regex assert_ex = new Regex(@"\[##.*?##\]");
            var match_collection = assert_ex.Matches(post.Content);
            foreach (var match in match_collection)
            {
                string test_string = match.ToString();
                Debug.Assert(test_string.StartsWith("[##_1C|") |
                             test_string.StartsWith("[##_1L|") |
                             test_string.StartsWith("[##_1R|"));
                Debug.Assert(test_string.EndsWith("_##]"));
            }
        }

        // 모든 이미지 파일이 실제로 백업 파일 안에 존재하는지, 그 반대도 존재하는지 체크한다
        private static void AssertReferencingImageFileExsit(Post post, string converted_content)
        {
            Regex image_tag = new Regex("<img src=\\\"(.*?)\\\"");
            var match_collection = image_tag.Matches(converted_content);
            List<string> referencing_filenames = new List<string>();
            foreach (Match match in match_collection)
            {
                referencing_filenames.Add(match.Groups[1].Value);
            }
            referencing_filenames.Sort();

            List<string> attachment_filenames = new List<string>();
            foreach (var attachment in post.AttachmentFiles)
            {
                attachment_filenames.Add(attachment.Label);
            }
            attachment_filenames.Sort();

            if (referencing_filenames.Count == attachment_filenames.Count)
            {
                return;
            }
            else if (referencing_filenames.Count > attachment_filenames.Count)
            {
                // 참조한 숫자가 더 많으면, 참조가 모두 제대로 참조하는 지를 검사한다
                foreach (string referencing_filename in referencing_filenames)
                {
                    Debug.Assert(attachment_filenames.Contains(referencing_filename));
                }
            }
            else if (referencing_filenames.Count < attachment_filenames.Count)
            {
                // 참조한 숫자가 더 적으면, 참조하지 않은 파일을 본문 아래에 첨부 파일을 넣어줘야 한다.... 구현 중...
                foreach (string attachment_filename in attachment_filenames)
                {
                    if (referencing_filenames.Contains(attachment_filename) == false)
                    {
                        Console.WriteLine("Missing reference filename: " + attachment_filename);
                    }
                }
            }
        }

        private static string ReplaceImagePart(Post post)
        {
            string replaced = post.Content;
            // 가끔 label이 아니라 name으로 참조되어있는 것들은 먼저 label로 변환시킨다
            foreach (var attachment in post.AttachmentFiles)
            {
                replaced = replaced.Replace(attachment.Name, attachment.Label);
            }
            return TistoryImageReplacerToHtmlImageTag(replaced);
        }

        public static string TistoryImageReplacerToHtmlImageTag(string content)
        {
            string replaced = content;

            {
                Regex image_regex = new Regex("\\[##_1.*?width=\\\"(.*?)\\\" height=\\\"(.*?)\\\".*?filename=\\\"(.*?)\\\".*?##\\]");
                replaced = image_regex.Replace(replaced, "<img src=\"$3\" width=\"$1\" height=\"$2\" />");
            }
            {
                Regex image_regex = new Regex("\\[##_1.\\|(.*?)\\|width=\\\"(.*?)\\\" height=\\\"(.*?)\\\".*?##\\]");
                replaced = image_regex.Replace(replaced, "<img src=\"$1\" width=\"$2\" height=\"$3\" />");
            }
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
