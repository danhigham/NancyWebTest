using System.IO;

namespace NancyWebTest
{
    public interface IImageStore
    {
        void Save(string filename, Stream filestream, string contentType);

    }
}
