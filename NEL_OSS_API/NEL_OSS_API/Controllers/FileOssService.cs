using Microsoft.Extensions.Options;
using NEL.helper;
using NEL_OSS_API;
using System;

namespace FileService.Controllers
{
    public class FileOssService
    {
        public FileOssRes ossUpload(FileOssReq req)
        {
            string fileName = getTmpFileName(req.FileName);
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

        public FileOssRes ossStore(FileOssReq req)
        {
            string fileName = getTmpFileName(req.FileName);
            // 参数检查
            if (string.IsNullOrEmpty(fileName)) return newFailedRes(RespCode.FILE_OP_SERVICE_IllegalParameter, "参数不能为空(fileName)");

            string sourceObject = fileName;
            string targetObject = fileName.Substring(4);
            string res = "false";
            try
            {
                res = client.CopyObject(sourceObject, targetObject);
            }
            catch (Exception ex)
            {
                ErrorLog(ex);
                return newFailedRes(RespCode.FILE_OP_SERVICE_InternalError, ex.Message);
            }
            return newSuccessRes(res);
        }
        
        public FileOssRes ossDelete(FileOssReq req)
        {
            string fileName = getTmpFileName(req.FileName);
            // 参数检查
            if (string.IsNullOrEmpty(fileName)) return newFailedRes(RespCode.FILE_OP_SERVICE_IllegalParameter, "参数不能为空(fileName)");

            string res = "false";
            try
            {
                res = client.DeleteObject(fileName);
            }
            catch (Exception ex)
            {
                ErrorLog(ex);
                return newFailedRes(RespCode.FILE_OP_SERVICE_InternalError, ex.Message);
            }
            return newSuccessRes(res);
        }
        
        private string getTmpFileName(string filename)
        {
            return "tmp_" + filename;
        }

        private FileOssRes newFailedRes(String code, String errMsg)
        {
            return new FileOssRes(code, errMsg, "");
        }
        private FileOssRes newSuccessRes(string data)
        {
            return new FileOssRes(RespCode.SUCCESS, "", data == null ? "" : data.ToString());
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

        private static FileOssService fileOssService = new FileOssService();
        public static FileOssService getInstance() { return fileOssService; }
        private FileOssService()
        {
            client = new FileOssClient(new FileOssConfig
            {
                AccessKeyId = Config.param.AccessKeyId,
                AccessKeySecret = Config.param.AccessKeySecret,
                Endpoint = Config.param.Endpoint,
                BucketName = Config.param.BucketName,
            });
        }
        private FileOssClient client;

        public static void ErrorLog(Exception ex)
        {
            
            LogHelper.printEx(ex);
        }
    }
}
