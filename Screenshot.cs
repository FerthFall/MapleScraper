using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace MapleScreenScraper
{
    internal static class Screenshot
    {
        public static Bitmap TakeScreenshot(Rectangle region)
        {
            Bitmap screenshot = new Bitmap(region.Width, region.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(region.Location, Point.Empty, region.Size);
            }

            return ScaleBitmap(screenshot, 3);
        }

        private static Bitmap ScaleBitmap(Bitmap bitmap, int scaleFactor)
        {
            int newWidth = bitmap.Width * scaleFactor;
            int newHeight = bitmap.Height * scaleFactor;

            Bitmap newBitmap = new Bitmap(newWidth, newHeight);

            using (Graphics graphics = Graphics.FromImage(newBitmap))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphics.DrawImage(bitmap, 0, 0, newWidth, newHeight);
            }

            return newBitmap;
        }

        public static string ReadTextFromBitmap(Bitmap bitmap)
        {
            using (var ocr = new Tesseract.TesseractEngine("./tessdata", "eng", Tesseract.EngineMode.Default))
            {
                ocr.SetVariable("tessedit_char_whitelist", "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
                ocr.SetVariable("tessedit_unrej_any_wd", true);

                using (var page = ocr.Process(bitmap))
                {
                    return page.GetText();
                }
            }
        }

        static Bitmap ConvertToGrayscale(Bitmap bitmap)
        {
            // Create a new grayscale bitmap image with the same dimensions as the original image
            Bitmap grayscaleBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            // Loop through all the pixels in the original bitmap image
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    // Get the color of the current pixel in the original bitmap image
                    Color originalColor = bitmap.GetPixel(x, y);

                    // Calculate the grayscale value for the current pixel using the luminosity method
                    int grayscaleValue = (int)(0.21 * originalColor.R + 0.72 * originalColor.G + 0.07 * originalColor.B);

                    // Create a new color with the grayscale value for the current pixel
                    Color grayscaleColor = Color.FromArgb(grayscaleValue, grayscaleValue, grayscaleValue);

                    // Set the color of the current pixel in the grayscale bitmap image
                    grayscaleBitmap.SetPixel(x, y, grayscaleColor);
                }
            }

            // Save the grayscale bitmap image
            return grayscaleBitmap;
        }

    }
}
