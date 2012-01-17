using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace NancyWebTest
{
    public class AzureStore : IImageStore
    {
        public void Save(string filename, Stream filestream)
        {
            var client = CloudStorageAccountStorageClientExtensions.CreateCloudBlobClient(new CloudStorageAccount(new StorageCredentialsAccountAndKey("two10ra", "dmIMUY1mg/qPeOgGmCkO333L26cNcnUA1uMcSSOFMB3cB8LkdDkh02RaYTPLBL8qMqnqazqd6uMxI2bJJEnj0g=="), false));
            client.GetContainerReference("imageuploads").GetBlobReference(filename).UploadFromStream(filestream);
        }
    }
}
