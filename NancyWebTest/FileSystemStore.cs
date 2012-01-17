using System.IO;

namespace NancyWebTest
{
    public class FileSystemStore : IImageStore
    {
        public void Save(string filename, Stream filestream)
        {
            using (Stream file = File.OpenWrite(filename))
            {
                CopyStream(filestream, file);
            }

        }

        static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[4 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

    }
}
