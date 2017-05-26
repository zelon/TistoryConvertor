using System;
using System.Collections.Generic;
using System.Text;

namespace TistoryConvertor
{
    class AttachmentFile
    {
        public string Mime { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
    }
}
