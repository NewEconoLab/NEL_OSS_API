using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileService.Controllers
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

        private FileOssService client;
        public TestnetController(IOptions<FileOssConfig> setting)
        {
            Console.WriteLine("TestController init");
            client = new FileOssService(setting, "testnet");
        }
    }
}
