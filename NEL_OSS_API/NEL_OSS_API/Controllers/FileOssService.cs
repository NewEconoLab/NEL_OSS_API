using Microsoft.Extensions.Options;
using System;

namespace FileService.Controllers
{
    public class FileOssService
    {
        public FileOssRes ossUpload(FileOssReq req)
        {
            string fileName = req.FileName;
            string fileContent = req.FileContent;

            // 参数检查
            if (string.IsNullOrEmpty(fileName)) return newFailedRes(RespCode.FILE_OP_SERVICE_IllegalParameter, "参数不能为空(fileName)");
            if (string.IsNullOrEmpty(fileContent)) return newFailedRes(RespCode.FILE_OP_SERVICE_IllegalParameter, "参数不能为空(fileContent)");

            // 上传文件内容
            string res = "false";
            try
            {
                res = client.PutObject(fileName, fileContent);
            } catch (Exception ex)
            {
                ErrorLog(ex);
                return newFailedRes(RespCode.FILE_OP_SERVICE_InternalError, ex.Message);
            }

            //
            return newSuccessRes(res);
        }
        
        public FileOssRes ossDownload(FileOssReq req)
        {
            string fileName = req.FileName;

            // 参数检查
            if (string.IsNullOrEmpty(fileName)) return newFailedRes(RespCode.FILE_OP_SERVICE_IllegalParameter, "参数不能为空(fileName)");

            // 获取文件内容
            string fileContent = null;
            try
            {
                fileContent = client.GetObject(fileName);
            } catch (Exception ex)
            {
                ErrorLog(ex);
                return newFailedRes(RespCode.FILE_OP_SERVICE_InternalError, ex.Message);
            }
            //
            return newSuccessRes(fileContent);
        }

        private FileOssRes newFailedRes(String code, String errMsg)
        {
            return new FileOssRes(code, errMsg, "");
        }
        private FileOssRes newSuccessRes(string data)
        {
            return new FileOssRes(RespCode.SUCCESS, "", data == null ? "" : data.ToString());
        }

        private FileOssClient client;
        public FileOssService(IOptions<FileOssConfig> setting, string network)
        {
            client = new FileOssClient(setting.Value, network);
        }

        internal class RespCode
        {
            // 成功
            public static string SUCCESS = "000000";

            // 内部错误(003101~003199)
            public static string FILE_OP_SERVICE_InternalError = "003101";

            // 参数错误(003201~003299)
            public static string FILE_OP_SERVICE_IllegalParameter = "003201";
            
        }


        public static void ErrorLog(Exception ex)
        {
            System.Text.StringBuilder msg = new System.Text.StringBuilder();
            msg.Append("*************************************** \n");
            msg.AppendFormat(" 异常发生时间： {0} \n", DateTime.Now);
            msg.AppendFormat(" 异常类型： {0} \n", ex.HResult);
            msg.AppendFormat(" 导致当前异常的 Exception 实例： {0} \n", ex.InnerException);
            msg.AppendFormat(" 导致异常的应用程序或对象的名称： {0} \n", ex.Source);
            msg.AppendFormat(" 引发异常的方法： {0} \n", ex.TargetSite);
            msg.AppendFormat(" 异常堆栈信息： {0} \n", ex.StackTrace);
            msg.AppendFormat(" 异常消息： {0} \n", ex.Message);
            msg.Append("***************************************");
            Console.WriteLine(msg.ToString());
        }
    }
}
