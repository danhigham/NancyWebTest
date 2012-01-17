using System;
using System.Drawing.Imaging;
using System.IO;
using Nancy;

namespace NancyWebTest
{
    public class ImageUploadModule : NancyModule
    {
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

        public void ResizeImage(Stream original, string NewFile, int NewWidth, int MaxHeight, bool OnlyResizeIfWider)
        {
            System.Drawing.Image FullsizeImage = System.Drawing.Image.FromStream(original);

            // Prevent using images internal thumbnail
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            if (OnlyResizeIfWider)
            {
                if (FullsizeImage.Width <= NewWidth)
                {
                    NewWidth = FullsizeImage.Width;
                }
            }

            int NewHeight = FullsizeImage.Height * NewWidth / FullsizeImage.Width;
            if (NewHeight > MaxHeight)
            {
                // Resize with height instead
                NewWidth = FullsizeImage.Width * MaxHeight / FullsizeImage.Height;
                NewHeight = MaxHeight;
            }

            System.Drawing.Image NewImage = FullsizeImage.GetThumbnailImage(NewWidth, NewHeight, null, IntPtr.Zero);

            // Clear handle to original file so that we can overwrite it if necessary
            FullsizeImage.Dispose();

            // Save resized picture
            NewImage.Save(NewFile, ImageFormat.Png);
        }
    }
}
