using System;
using System.IO;

namespace NancyWebTest
{
    public class S3Store : IImageStore
    {
        public void Save(string filename, Stream filestream)
        {
            throw new NotImplementedException("Add your code to save to S3 storage here");

        }



    }
}
