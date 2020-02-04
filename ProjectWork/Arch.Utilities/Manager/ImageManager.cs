using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
namespace Arch.Utilities.Manager
{
    public static class ImageManager
    {
        public static string GetMimeType(ImageFormat imageFormat)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.First(codec => codec.FormatID == imageFormat.Guid).MimeType;
        }
        public static byte[] imageToByteArray(System.Drawing.Image imageIn, string contentType)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, contentTypetoImageFormat(contentType));
            return ms.ToArray();
        }

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = new Bitmap(new MemoryStream(byteArrayIn));
            return returnImage;
        }

        public static byte[] byteArrayRotate(byte[] byteArrayIn, string contentType, RotateFlipType rft)
        {
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(memoryStream);
            memoryStream.Close();
            returnImage.RotateFlip(rft);

            return imageToByteArray(returnImage, contentType);
        }


        static string[] imgFormatsString = { "bmp", "emf", "exif", "gif", "icon", "jpeg", "memorybmp", "png", "tiff", "wmf" };
        static ImageFormat[] imgFormats = { ImageFormat.Bmp, ImageFormat.Emf, ImageFormat.Exif, ImageFormat.Gif, ImageFormat.Icon, ImageFormat.Jpeg, ImageFormat.MemoryBmp, ImageFormat.Png, ImageFormat.Tiff, ImageFormat.Wmf };

        public static ImageFormat contentTypetoImageFormat(string contentType)
        {
            for (int i = 0; i < imgFormatsString.Length; i++)
            {
                if (imgFormatsString[i] == contentType.Split("/".ToCharArray())[1].ToString())
                    return imgFormats.ToList()[i];
            }
            return null;
        }

        public static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static string SizeSuffix(int value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue / 1024) >= 1)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n1} {1}", dValue, SizeSuffixes[i]);
        }

        public static Image ScaleBySize(Image imgPhoto, int Width, int Height, bool needToFill)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            if ((float)sourceWidth > (float)sourceHeight)
            {
                nPercentW = ((float)Width / (float)sourceWidth); nPercentH = ((float)Width / (float)sourceWidth);
            }
            else
            {
                nPercentH = ((float)Height / (float)sourceHeight); nPercentW = ((float)Height / (float)sourceHeight);
            }
            if (!needToFill)
            {
                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentH;
                }
                else
                {
                    nPercent = nPercentW;
                }
            }
            else
            {
                if (nPercentH > nPercentW)
                {
                    nPercent = nPercentH;
                    destX = (int)Math.Round((Width -
                        (sourceWidth * nPercent)) / 2);
                }
                else
                {
                    nPercent = nPercentW;
                    destY = (int)Math.Round((Height -
                        (sourceHeight * nPercent)) / 2);
                }
            }
            if (nPercent > 1)
                nPercent = 1;
            int destWidth = (int)Math.Round(sourceWidth * nPercent);
            int destHeight = (int)Math.Round(sourceHeight * nPercent);
            Bitmap bmPhoto = new Bitmap(imgPhoto,
                destWidth <= Width ? destWidth : Width,
                destHeight < Height ? destHeight : Height);
            return bmPhoto;
        }


        public static Image Crop(Image img, Rectangle cropArea, bool customCrop)
        {
            var bmpImage = new Bitmap(img);
            var bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }
        static string[] iconArray = { "fa fa-file-excel-o", "fa fa-file-excel-o", "fa fa-file-pdf-o", "fa fa-file-word-o", "fa fa-file-powerpoint-o", "fa fa-file-zip-o", "fa fa-file-image-o", "fa fa-file-text", "fa fa-file-audio-o", "fa fa-file-video-o", "fa fa-file-o" };
        static string[] fileContentTypeArray = { "excel", "sheet", "pdf", "word", "presentation", "octet-stream", "image", "text", "audio", "video", "access" };
        public static string iconSet(string dosyaUzantisi)
        {
            for (int i = 0; i < iconArray.Length; i++)
                if (dosyaUzantisi.Contains(fileContentTypeArray[i]))
                    return iconArray[i];
            return "";
        }

        public static byte[] ConvertToSize(byte[] image, int? w, int? h, string contentType)
        {

            bool noCrop = false;
            w = w == 0 ? null : w;
            h = h == 0 ? null : h;
            if ((w != null || h != null))
            {
                System.Drawing.Image orjImg = byteArrayToImage(image);
                if (h == null)
                {
                    h = Convert.ToInt32(((float)w / (float)orjImg.Width) * orjImg.Height);
                    noCrop = true;
                }
                if (w == null)
                {
                    w = Convert.ToInt32(((float)h / (float)orjImg.Height) * orjImg.Width);
                    noCrop = true;
                }
                if (noCrop)
                {
                    Bitmap bmPhoto = new Bitmap(orjImg, w.Value, h.Value);
                    image = imageToByteArray((System.Drawing.Image)bmPhoto, contentType);
                }
                else
                {
                    float oran = (float)w.Value / (float)h.Value;
                    float orjOran = (float)orjImg.Width / (float)orjImg.Height;
                    int x = 0, y = 0;
                    Rectangle cropArea = new Rectangle();

                    if (oran < orjOran)
                    {
                        cropArea.X = (int)((orjImg.Width - oran * orjImg.Height) / 2);
                        cropArea.Y = 0;
                        cropArea.Height = orjImg.Height;
                        cropArea.Width = (int)(oran * orjImg.Height);
                    }
                    else
                    {
                        cropArea.X = 0;
                        cropArea.Y = (int)((orjImg.Height - orjImg.Width / oran) / 2);
                        cropArea.Height = (int)(orjImg.Width / oran);
                        cropArea.Width = orjImg.Width;
                    }
                    var bmpImage = new Bitmap(orjImg);
                    var cbmpImage = new Bitmap(orjImg, w.Value, h.Value);
                    Graphics grp = Graphics.FromImage(cbmpImage);
                    grp.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    grp.Clear(Color.Transparent);
                    grp.DrawImage(bmpImage, new Rectangle(0, 0, w.Value, h.Value), new Rectangle(cropArea.X, cropArea.Y, cropArea.Width, cropArea.Height), GraphicsUnit.Pixel);
                    grp.Dispose();
                    image = imageToByteArray((System.Drawing.Image)cbmpImage, contentType);
                }
            }

            return image;
        }
    }
}
