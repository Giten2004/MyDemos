using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliOSS
{
    /// <summary>
    /// https://github.com/aliyun/aliyun-oss-csharp-sdk
    /// </summary>
    public static class SimpleSamples
    {
        private const string _accessKeyId = "JgXJblW4uzMK4iwO";
        private const string _accessKeySecret = "tIgG3ny7zMI2P8ZYMx7sUyTNxIDX8z";

        //OSS外网域名: bxutestdemo.oss-cn-hangzhou.aliyuncs.com
        private const string _endpoint = "oss-cn-hangzhou.aliyuncs.com";
        private const string _bucketName = "bxutestdemo";

        //文件的在OSS上保存的名称
        private const string _key = "winhlp32.exe";
        private const string _fileToUpload = @"C:\Windows\winhlp32.exe";
        

        private static OssClient _client = new OssClient(_endpoint, _accessKeyId, _accessKeySecret);

        public static void Main(string[] args)
        {
            //CreateBucket();
            SetBucketAcl();
            GetBucketAcl();
            PutObject();
            ListObjects();
            GetObject();
            //DeleteObject();
            // DeleteBucket();
            Console.WriteLine("Press any key to continue . . . ");
            Console.ReadKey(true);
        }
        /// <summary>
        /// 创建一个新的存储空间
        /// </summary>
        private static void CreateBucket()
        {
            try
            {
                var result = _client.CreateBucket(_bucketName);
                Console.WriteLine("创建存储空间{0}成功", result.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("创建存储空间失败. 原因：{0}", ex.Message);
            }
        }
        /// <summary>
        /// 上传一个新文件
        /// </summary>
        private static void PutObject()
        {
            try
            {
                _client.PutObject(_bucketName, _key, _fileToUpload);
                Console.WriteLine("上传文件成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine("上传文件失败.原因: {0}", ex.Message);
            }
        }
        /// <summary>
        /// 列出存储空间内的所有文件
        /// </summary>
        private static void ListObjects()
        {
            try
            {
                var keys = new List<string>();
                ObjectListing result = null;
                string nextMarker = string.Empty;
                /// 由于ListObjects每次最多返回100个结果，所以，这里需要循环去获取，直到返回结果中IsTruncated为false
                do
                {
                    var listObjectsRequest = new ListObjectsRequest(_bucketName)
                    {
                        Marker = nextMarker,
                        MaxKeys = 100
                    };
                    result = _client.ListObjects(listObjectsRequest);
                    foreach (var summary in result.ObjectSummaries)
                    {
                        keys.Add(summary.Key);
                    }
                    nextMarker = result.NextMarker;
                } while (result.IsTruncated);
                Console.WriteLine("列出存储空间中的文件");
                foreach (var key in keys)
                {
                    Console.WriteLine("文件名称：{0}", key);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("列出存储空间中的文件失败.原因： {0}", ex.Message);
            }
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        private static void GetObject()
        {
            try
            {
                var result = _client.GetObject(_bucketName, _key);
                Console.WriteLine("下载的文件成功，名称是：{0}，长度：{1}", result.Key, result.Metadata.ContentLength);
            }
            catch (Exception ex)
            {
                Console.WriteLine("下载文件失败.原因：{0}", ex.Message);
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        private static void DeleteObject()
        {
            try
            {
                _client.DeleteObject(_bucketName, _key);
                Console.WriteLine("删除文件成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine("删除文件失败.原因： {0}", ex.Message);
            }
        }
        /// <summary>
        /// 获取存储空间ACL的值
        /// </summary>
        private static void GetBucketAcl()
        {
            try
            {
                var result = _client.GetBucketAcl(_bucketName);
                foreach (var grant in result.Grants)
                {
                    Console.WriteLine("获取存储空间权限成功，当前权限:{0}", grant.Permission.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取存储空间权限失败.原因： {0}", ex.Message);
            }
        }
        /// <summary>
        /// /// 设置存储空间ACL的值
        /// </summary>
        private static void SetBucketAcl()
        {
            try
            {
                _client.SetBucketAcl(_bucketName, CannedAccessControlList.PublicRead);
                Console.WriteLine("设置存储空间权限成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine("设置存储空间权限失败. 原因：{0}", ex.Message);
            }
        }
        /// <summary>
        /// 删除存储空间
        /// </summary>
        private static void DeleteBucket()
        {
            try
            {
                _client.DeleteBucket(_bucketName);
                Console.WriteLine("删除存储空间成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine("删除存储空间失败", ex.Message);
            }
        }
    }
}