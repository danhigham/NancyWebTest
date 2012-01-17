using System;
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
                return View["index", Request.Url];
            };

            Post["/"] = parameters =>
            {

                foreach (var file in Request.Files)
                {
                    var imgPath = "temp-resized.png";

                    ResizeImage(file.Value, imgPath, 300, 200, true);
                }

                return View["index", Request.Url];
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
                kernel.Get<ImageSaver>().Save(newFile, s);
            }
        }
    }
}
