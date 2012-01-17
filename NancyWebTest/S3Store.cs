using System;
using System.IO;
using LitS3;

namespace NancyWebTest
{
    public class S3Store : IImageStore
    {
        private S3Service _S3Service;
        private const string _bucketName = "Nancy-Images";
        
        public S3Store()
        {
            _S3Service = new S3Service()
            {
                AccessKeyID = System.Environment.GetEnvironmentVariable("AMAZON_ACCESS_KEY_ID"),
                SecretAccessKey = System.Environment.GetEnvironmentVariable("AMAZON_SECRET_ACCESS_KEY")
            };
            
            if (_S3Service.QueryBucket(_bucketName) == BucketAccess.NoSuchBucket)
                _S3Service.CreateBucket(_bucketName);
        }
        
        public void Save(string filename, Stream filestream, string contentType)
        {
            try
            {
                _S3Service.AddObject(filestream, _bucketName, filename, contentType, CannedAcl.PublicRead);
                
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
            }      

        }



    }
}
