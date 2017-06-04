using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace TistoryConvertor
{
    class ParseUtil
    {
        public static Post ParsePost(XmlNode xml_node)
        {
            Post post = new Post();
            post.Id = int.Parse(xml_node["id"].InnerText);
            post.Title = xml_node["title"].InnerText;
            post.Content = xml_node["content"].InnerText;
            post.PublishedTime = ParseDateTime(xml_node["published"].InnerText);
            post.CreatedTime = ParseDateTime(xml_node["created"].InnerText);
            post.ModifiedTime = ParseDateTime(xml_node["modified"].InnerText);
            post.AttachmentFiles = ParseAttachments(xml_node);
            return post;
        }

        private static List<AttachmentFile> ParseAttachments(XmlNode post_node)
        {
            List<AttachmentFile> output = new List<AttachmentFile>();
            foreach (XmlNode node in post_node.ChildNodes)
            {
                if (node.Name != "attachment")
                {
                    continue;
                }
                output.Add(ParseAttachment(node));
            }
            return output;
        }

        private static AttachmentFile ParseAttachment(XmlNode node)
        {
            string real_filename = node["label"].InnerText;
            Debug.Assert(node["name"].InnerText.Length > 0);
            Debug.Assert(real_filename.ToLower().EndsWith("png") |
                         real_filename.ToLower().EndsWith("jpg"));


            var attachment_file = new AttachmentFile();
            attachment_file.Name = node["name"].InnerText;
            attachment_file.Label = node["label"].InnerText;
            attachment_file.Mime = node.Attributes["mime"].InnerText;
            attachment_file.Width = int.Parse(node.Attributes["width"].InnerText);
            attachment_file.Height = int.Parse(node.Attributes["height"].InnerText);
            attachment_file.Content = Convert.FromBase64String(node["content"].InnerText);
            return attachment_file;
        }

        public static DateTime ParseDateTime(string s)
        {
            long numeric = long.Parse(s);
            var offset = DateTimeOffset.FromUnixTimeSeconds(numeric).AddHours(9);
            DateTime output = offset.DateTime;
            return output;
        }
    }
}
