using System;

namespace FileService.Controllers
{
    public class FileOssRes
    {
        // 错误代码
        public string Code { get; }

        // 错误描述
        public string ErrMsg { get; }

        // 实体数据
        public Object Data { get; set; }

        
        public FileOssRes(string code, string errMsg, Object data)
        {
            Code = code;
            ErrMsg = errMsg;
            Data = data;
        }
    }
}
