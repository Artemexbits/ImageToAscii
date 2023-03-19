using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace ImageToAscii;
class Program
{
    static void Main(string[] args)
    {
        const int size = 150;
        const int quality = 75;
        string filepath = System.AppContext.BaseDirectory + "NewCaledonianCrow.jpg";
    
        Console.WriteLine("PATH: " + filepath);
        try {
        using (var image = new Bitmap(System.Drawing.Image.FromFile(filepath)))
        {
            int width, height;
            if (image.Width > image.Height)
            {
                width = size;
                height = Convert.ToInt32(image.Height * size / (double)image.Width);
            }
            else
            {
                width = Convert.ToInt32(image.Width * size / (double)image.Height);
                height = size;
            }
            var resized = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(resized))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(image, 0, 0, width, height);
                using (var output = File.Open("newImage.jpg", FileMode.Create))
                {
                    var qualityParamId = Encoder.Quality;
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                    var codec = ImageCodecInfo.GetImageDecoders()
                        .FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
                    resized.Save(output, codec, encoderParameters);
                }
            }
        }
        } catch (Exception e) {
            Console.WriteLine("ERROR: " + e);
        }
    }
}
