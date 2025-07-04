using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    internal class ChatIO
    {
        public string FileSender {  get; set; }
        public string FileReceiver { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public string FileContentBase64 { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
