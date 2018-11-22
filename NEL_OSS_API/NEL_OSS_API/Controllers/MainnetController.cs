using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileService.Controllers
{
    [Route("api/[controller]")]
    public class MainnetController : Controller
    {
        [HttpGet("oss/test")]
        public string test()
        {
            return "Welcome to Mainnet.OssFileService..." + new Random().Next();
        }

        [HttpPost("oss/upload")]
        public FileOssRes ossUpload([FromBody] FileOssReq req)
        {
            return client.ossUpload(req);
        }
        [HttpPost("oss/download")]
        public FileOssRes ossDownload([FromBody] FileOssReq req)
        {
            return client.ossDownload(req);
        }
        
        private FileOssService client;
        public MainnetController(IOptions<FileOssConfig> setting)
        {
            client = new FileOssService(setting, "mainnet");
        }
    }
}
