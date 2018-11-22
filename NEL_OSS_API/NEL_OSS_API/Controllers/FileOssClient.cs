using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileService.Controllers
{
    public class FileOssClient
    {
        /// <summary>
        /// 对象存储 SDK
        /// </summary>
        private OssClient client;

        /// <summary>
        /// 对象存储存储空间
        /// </summary>
        private string bucketName;

        public FileOssClient(FileOssConfig config) : this(config, "testnet") { }
        
        public FileOssClient(FileOssConfig config, string network)
        {
            string endpoint = config.Endpoint;
            string accessKeyId = config.AccessKeyId;
            string accessKeySecret = config.AccessKeySecret;
            client = new OssClient(endpoint, accessKeyId, accessKeySecret);

            if("mainnet" == network)
            {
                bucketName = config.BucketName_mainnet; // 主网bucket
            } else
            {
                bucketName = config.BucketName_testnet; // 测试bucket
            }
            
            try
            {
                bool isCreateBucket = true;
                foreach (Bucket bucket in client.ListBuckets())
                {
                    if(bucket != null && bucketName.Equals(bucket.Name) )
                    {
                        isCreateBucket = false;
                        break;
                    }
                }
                //if (client.ListBuckets().Where(p => p.Equals(bucketName)).First() == null)
                if (isCreateBucket)
                {
                    client.CreateBucket(bucketName);
                }

            } catch (Exception ex)
            {
                Console.WriteLine("exMsg:" + ex.Message);
            }
            
        }

        public string PutObject(string type, string version, string content)
        {
            return PutObject(getKey(type, version), content);
        }
        public string PutObject(string filename, string content)
        {
            byte[] binaryData = Encoding.ASCII.GetBytes(content);
            var stream = new MemoryStream(binaryData);
            client.PutObject(bucketName, filename, stream);
            return "true";

        }

        public string GetObject(string type, string version)
        {
            return GetObject(bucketName, getKey(type, version));
        }
        public string GetObject(string filename)
        {
            StringBuilder sb = new StringBuilder();
            var result = client.GetObject(bucketName, filename);
            using (var requestStream = result.Content)
            {
                int length = 4 * 1024;
                var buf = new byte[length];
                while (true)
                {
                    length = requestStream.Read(buf, 0, length);
                    if (length == 0) break;
                    sb.Append(Encoding.ASCII.GetString(buf.Take(length).ToArray()));
                }
            }
            return sb.ToString();

        }

        public string GetVersion(string type)
        {
            var result = client.ListObjects(bucketName);

            List<OssObjectSummary> list = null;
            list = result.ObjectSummaries.Where(p => p.Key.StartsWith(type)).ToList();
            if (list == null || list.Count == 0)
            {
                return "errMsg:Not find data";
            }
            list.Sort(new OssObjectComparator());

            return getVersion(list.FirstOrDefault().Key);
        }


        private string getKey(string type, string version)
        {
            return type + "_" + version + ".bin";
        }
        private string getVersion(string key)
        {
            int st = key.LastIndexOf("_");
            int ed = key.LastIndexOf(".");
            return key.Substring(st + 1, ed - st - 1);
        }

    }

    class OssObjectComparator : IComparer<OssObjectSummary>
    {
        public int Compare(OssObjectSummary x, OssObjectSummary y)
        {
            return x.Key.CompareTo(y.Key);
        }
    }
}
