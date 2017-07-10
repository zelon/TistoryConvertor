using System;
using System.IO;

namespace TistoryConvertor.PostOutput
{
    class Wimyblog : IPostOutput
    {
        public string OutputDirectoryName { get; set; }

        public Wimyblog(string output_directory_name)
        {
            OutputDirectoryName = output_directory_name;

            if (Directory.Exists(output_directory_name) == false)
            {
                Directory.CreateDirectory(output_directory_name);
            }
        }

        public void OnPost(Post post)
        {
            Console.WriteLine("Converting Id:{0} Title:{1}", post.Id, post.Title);

            Directory.CreateDirectory(GetPostDirectory(post));

            WriteMetaFile(post);
            WriteMarkdownFile(post);
            WriteAttachments(post);
        }

        private void WriteMetaFile(Post post)
        {
            string data = string.Format(
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine +
                "<metadata>" + Environment.NewLine +
                "  <title>{0}</title>" + Environment.NewLine +
                "  <created_time>{1}</created_time>" + Environment.NewLine +
                "</metadata>", post.Title, post.PublishedTime.ToString(Program.DateTimeFormat));

            WriteFileContent(post.Id, "metadata.xml", data);
        }

        private void WriteMarkdownFile(Post post)
        {
            WriteFileContent(post.Id, "index.md", MarkdownConvertor.ToMarkdown(post));
        }

        private void WriteAttachments(Post post)
        {
            foreach (var attachment in post.AttachmentFiles)
            {
                string output_filename = Path.Combine(OutputDirectoryName, post.Id.ToString(), attachment.Label);
                using (var file_stream = File.OpenWrite(output_filename))
                {
                    file_stream.Write(attachment.Content, 0, attachment.Content.Length);
                }
            }
        }

        private string GetPostDirectory(Post post)
        {
            return Path.Combine(OutputDirectoryName, post.Id.ToString());
        }

        private void WriteFileContent(int post_id, string filename, string content)
        {
            string output_filename = Path.Combine(OutputDirectoryName, post_id.ToString(), filename);
            using (var file_stream = File.CreateText(output_filename))
            {
                file_stream.Write(content);
            }
        }
    }
}
