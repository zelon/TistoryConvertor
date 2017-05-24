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
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("Title: {0}", post.Title);
            Console.WriteLine("PublishedTime: {0}", post.PublishedTime.ToString("u"));
            Console.WriteLine(post.Content);

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
                "</metadata>", post.Title, DateTime.Now.ToString(Program.DateTimeFormat));

            WriteFileContent(post.Id, "metadata.xml", data);
        }

        private void WriteMarkdownFile(Post post)
        {
            WriteFileContent(post.Id, "index.md", post.Content);
        }

        private void WriteAttachments(Post post)
        {

        }

        private string GetPostDirectory(Post post)
        {
            return Path.Combine(OutputDirectoryName, post.Id.ToString());
        }

        private void WriteFileContent(int post_id, string filename, string content)
        {
            string output_filename = Path.Combine(OutputDirectoryName, post_id.ToString(), filename);
            byte[] encoded_bytes = System.Text.Encoding.UTF8.GetBytes(content);
            using (var file_stream = File.OpenWrite(output_filename))
            {
                file_stream.Write(encoded_bytes, 0, encoded_bytes.Length);
            }
        }
    }
}
