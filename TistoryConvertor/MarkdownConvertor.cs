using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TistoryConvertor
{
    public class MarkdownConvertor
    {
        public static string ToMarkdown(Post post)
        {
            AssertAllImageReplacerMark(post);
            string converted = ReplaceImagePart(post);
            converted = Html2Markdown.Convert(converted);
            converted += GetNotReferencedAttachmentFilesBody(post);
            return converted;
        }

        private static string GetNotReferencedAttachmentFilesBody(Post post)
        {
            var not_referenced_attachment_filenames = GetNotReferencedAttachmentFilenames(post);
            if (not_referenced_attachment_filenames.Count == 0)
            {
                return "";
            }
            string output = Environment.NewLine + "첨부파일: ";
            foreach (var filename in not_referenced_attachment_filenames)
            {
                output += string.Format("<a href=\"{0}\">{0}</a> ", filename);
            }
            return output;
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
            // width, height에는 소수점이 있을 때도 있어서 \d 를 사용할 수 없다
            Regex image_regex = new Regex("\\[##_1.\\|(.*?)\\|width=\\\"(.*?)\\\" height=\\\"(.*?)\\\".*?##\\]");
            replaced = image_regex.Replace(replaced, "<img src=\"$1\" width=\"$2\" height=\"$3\" />");

            // replace하고 나면 [##로 시작하고 ##]로 끝나는 건 없어야 한다
            {
                Regex test_regex = new Regex("\\[##_.*?##\\]");
                Debug.Assert(test_regex.IsMatch(replaced) == false);
            }
            return replaced;
        }

        private static List<string> GetNotReferencedAttachmentFilenames(Post post)
        {
            Regex image_tag = new Regex("<img src=\\\"(.*?)\\\"");
            string converted_content = ReplaceImagePart(post);
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

            // 참조한 파일들이 존재하는 지 검사
            foreach (string referencing_filename in referencing_filenames)
            {
                Debug.Assert(attachment_filenames.Contains(referencing_filename));
            }

            // 첨부되었지만 참조하지 않은 파일들은 반환해준다
            List<string> to_be_attached_filenames = new List<string>();
            foreach (string attachment_filename in attachment_filenames)
            {
                if (referencing_filenames.Contains(attachment_filename) == false)
                {
                    to_be_attached_filenames.Add(attachment_filename);
                }
            }
            return to_be_attached_filenames;
        }
    }
}
