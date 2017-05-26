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
    }
}
