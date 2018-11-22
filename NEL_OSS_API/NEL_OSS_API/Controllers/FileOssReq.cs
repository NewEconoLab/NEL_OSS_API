using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileService.Controllers
{
    public class FileOssReq
    {
        // 文件名称
        public string FileName { get; set; }
        
        // 文件内容
        public string FileContent { get; set; }
    }
}
