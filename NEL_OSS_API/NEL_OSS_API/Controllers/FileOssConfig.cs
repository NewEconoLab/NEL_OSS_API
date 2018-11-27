using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileService.Controllers
{
    public class FileOssConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public string Endpoint { get; set; }

        public string AccessKeyId { get; set; }
        
        public string  AccessKeySecret { get; set; }

        public string BucketName { get; set; }
    }
}
