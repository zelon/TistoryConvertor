using System;
using System.Collections.Generic;
using System.Text;

namespace TistoryConvertor
{
    class AttachmentFile
    {
        public string Mime { get; set; }
        public string Name { get; set; }
        public string Label { get; set; } // 백업 파일에 왜 name, label이 따로 존재하는지는 모르겠지만, label이 좀 더 인식 가능한 이름이다
        public byte[] Content { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
    }
}
