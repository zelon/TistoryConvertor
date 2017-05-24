using System;
using System.Collections.Generic;
using System.Xml;

namespace TistoryConvertor
{
    class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public List<AttachmentFile> AttachmentFiles { get; set; }
        public List<Comment> Comments { get; set; }

        public static Post Parse(XmlNode xml_node)
        {
            Post post = new Post();
            post.Id = int.Parse(xml_node["id"].InnerText);
            post.Title = xml_node["title"].InnerText;
            post.Content = xml_node["content"].InnerText;
            post.PublishedTime = ParseUtil.ParseDateTime(xml_node["published"].InnerText);
            post.CreatedTime = ParseUtil.ParseDateTime(xml_node["created"].InnerText);
            post.ModifiedTime = ParseUtil.ParseDateTime(xml_node["modified"].InnerText);
            return post;
        }
    }
}
