using System;
using System.Diagnostics;
using System.IO;
using Nancy;
using Ninject;

namespace NancyWebTest
{
    public class ImageUploadModule : NancyModule
    {
        public IImageStore ImageStore { get; set; }

        public ImageUploadModule()
        {
            Get["/"] = parameters =>
            {
                return View["index.sshtml", new { Time = "" }];
            };

            Post["/"] = parameters =>
            {
                Stopwatch sw = Stopwatch.StartNew();
                foreach (var file in Request.Files)
                {
                    ResizeImage(file.Value, file.Name, 300, 200, true);
                }
                sw.Stop();

                return View["index.sshtml", new { Time = sw.ElapsedMilliseconds.ToString() + " milliseconds to crunch and store the image" }];
            };
        }

        public void ResizeImage(Stream original, string newFile, int newWidth, int maxHeight, bool onlyResizeIfWider)
        {
            System.Drawing.Image FullsizeImage = System.Drawing.Image.FromStream(original);

            // Prevent using images internal thumbnail
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            if (onlyResizeIfWider)
            {
                if (FullsizeImage.Width <= newWidth)
                {
                    newWidth = FullsizeImage.Width;
                }
            }

            int NewHeight = FullsizeImage.Height * newWidth / FullsizeImage.Width;
            if (NewHeight > maxHeight)
            {
                // Resize with height instead
                newWidth = FullsizeImage.Width * maxHeight / FullsizeImage.Height;
                NewHeight = maxHeight;
            }

            var NewImage = FullsizeImage.GetThumbnailImage(newWidth, NewHeight, null, IntPtr.Zero);

            // Clear handle to original file so that we can overwrite it if necessary
            FullsizeImage.Dispose();

            // Save resized picture
            using (Stream s = new MemoryStream())
            {
                NewImage.Save(s, System.Drawing.Imaging.ImageFormat.Png);
                s.Flush();
                s.Position = 0;
                var kernel = new StandardKernel(new NinjectModule());
                kernel.Get<IImageStore>().Save(newFile, s, "string/png");
            }
        }
    }
}
