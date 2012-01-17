using System;
using Nancy;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;


namespace NancyWebTest
{
	public class ImageUploadModule : NancyModule
	{
		public ImageUploadModule()
        {
            Get["/"] = parameters => {
                return View["index", Request.Url];
            };
			
			Post["/"] = parameters => {
				
                S3Uploader uploader = new S3Uploader();
                
				foreach(var file in Request.Files)
				{				
                    var originName = file.Name;
					var imgPath = String.Format("{0}temp-resized.png", Path.GetTempPath());
				   
					ResizeImage(file.Value, imgPath, 300, 200, true);
                    
                    uploader.UploadToBucket("nancy-images", imgPath, originName, "image/png");
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
