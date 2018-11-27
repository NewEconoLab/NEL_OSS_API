using System;
using Microsoft.AspNetCore.Mvc;
using NEL_OSS_API.RPC;
using NEL_OSS_API.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NEL_OSS_API.Controllers
{
    [Route("api/[controller]")]
    public class TestnetController : Controller
    {
        [HttpGet("oss/test")]
        public string test()
        {
            return "Welcome to Testnet.OssFileService..." + new Random().Next();
        }

        [HttpPost("oss/upload")]
        public FileOssRes ossUpload([FromBody] FileOssReq req)
        {
            return client.ossUpload(req);
            //return new FileOssRes("0001","upload-res","");
        }
        [HttpPost("oss/download")]
        public FileOssRes ossDownload([FromBody] FileOssReq req)
        {
            return client.ossDownload(req);
            //return new FileOssRes("0002", "download-res", "");
        }
        [HttpPost("oss/store")]
        public FileOssRes ossStore([FromBody] FileOssReq req)
        {
            return client.ossStore(req);
        }
        [HttpPost("oss/delete")]
        public FileOssRes ossDelete([FromBody] FileOssReq req)
        {
            return client.ossDelete(req);
        }

        private FileOssService client = FileOssService.getInstance();
    }
}
