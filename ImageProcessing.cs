using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace WinToolkit
{
	public enum PageType
	{
		A4,
	}
	public static class ImageProcessing
	{
		private static ColorMatrix colorMatrix;
		private static ImageAttributes attributes;

		public static Image RescaleImage(Image originalImage, Size newDimensions)
		{
			return RescaleImage(originalImage, newDimensions.Width, newDimensions.Height);
		}

		public static bool IsImageLandscape(Image img)
		{
			return img.Width > img.Height;
		}
		public static Image RescaleImage(Image originalImage, int width, int height)
		{
			if (originalImage == null || width == 0 || height == 0)
				return null;
			if (originalImage.Width == width && originalImage.Height == height) return originalImage;

			Bitmap finalImage = new Bitmap(width, height);
			using (Graphics graphics = Graphics.FromImage(finalImage))
			{
				//set the resize quality modes to high quality
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;

				//draw the image into the target bitmap
				graphics.DrawImage(originalImage, 0, 0, width, height);
			}

			return finalImage;
		}

		public static Image CropImage(Image originalImage, int width, int height)
		{
			if (originalImage == null || width == 0 || height == 0)
				return null;

			int scallingWidth = originalImage.Width;
			int scallingHeight = originalImage.Height;
			while (scallingWidth != width && scallingHeight != height)
			{
				scallingWidth--;
				scallingHeight--;
			}

			int factor = 1;
			if (scallingWidth > scallingHeight)
			{
				// factor depending on width
				factor = originalImage.Width / scallingWidth;
				scallingHeight = originalImage.Height / factor;
			}
			else
			{
				//factor depending on height
				factor = originalImage.Height / scallingHeight;
				scallingWidth = originalImage.Width / factor;
			}
			originalImage = RescaleImage(originalImage, scallingWidth, scallingHeight);
			int x = (originalImage.Width - width) / 2;
			int y = (originalImage.Height - height) / 2;
			Rectangle cropRect = new Rectangle(x, y, width, height);

			Bitmap finalImage = new Bitmap(width, height);
			using (Graphics graphics = Graphics.FromImage(finalImage))
			{
				//set the resize quality modes to high quality
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;

				//draw the image into the target bitmap
				graphics.DrawImage(originalImage, new Rectangle(0, 0, width, height), cropRect, GraphicsUnit.Pixel);
			}

			return finalImage;
		}

		public static ZoomAndMargin CalculateBestFitParams(Image imageToFitImage, Size containingRect)
		{
			if (imageToFitImage == null) return null;

			ZoomAndMargin result = new ZoomAndMargin();
			var currImgW = imageToFitImage.Width;
			int ratioW = (100 * containingRect.Width) / currImgW;
			var currImgH = imageToFitImage.Height;
			int ratioH = (100 * containingRect.Height) / currImgH;
			result.ZoomFactor = 0;
			result.Margin = 0;
			if (ratioH > ratioW)
			{
				result.ZoomFactor = ratioW;
				result.Margin = 13;
			}
			else if (ratioH < ratioW)
			{
				result.ZoomFactor = ratioH;
				result.Margin = 3;
			}

			return result;
		}

		private static ImageFormat GetImageFormat(string imageFullPath)
		{
			if (!File.Exists(imageFullPath)) return null;
			Image imageToCheck = Image.FromFile(imageFullPath);
			ImageFormat format = imageToCheck.RawFormat;
			imageToCheck.Dispose();

			return format;
		}

		private static ImageCodecInfo GetEncoder(ImageFormat format)
		{
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
			foreach (ImageCodecInfo codec in codecs)
				if (codec.FormatID == format.Guid)
					return codec;

			return null;
		}

		public static void SaveImageChanges(Image modifiedImage, string path)
		{
			Bitmap nuImage = new Bitmap(modifiedImage.Width, modifiedImage.Height, modifiedImage.PixelFormat);
			using (Graphics grph = Graphics.FromImage(nuImage))
			{
				grph.DrawImage(modifiedImage, 0, 0, modifiedImage.Width, modifiedImage.Height);
				modifiedImage.Dispose();	// Dispose old image first before overwriting

				nuImage.Save(path, GetImageFormat(path));
				nuImage.Dispose();
			}
		}

		public static Bitmap GetGrayScaleVerion(Image coloredImage)
		{
			if (coloredImage == null) return null;

			//create the gray-scale ColorMatrix
			if (colorMatrix == null)
			{
				colorMatrix = new ColorMatrix(
					new float[][] 
					{
						new float[] {.3f, .3f, .3f, 0, 0},
						new float[] {.59f, .59f, .59f, 0, 0},
						new float[] {.11f, .11f, .11f, 0, 0},
						new float[] {0, 0, 0, 1, 0},
						new float[] {-0.3f, -0.3f, -0.3f, 0, 1}
					});

				//create image attributes
				attributes = new ImageAttributes();
				attributes.SetColorMatrix(colorMatrix);
			}

			Bitmap grayscaleBitmap = new Bitmap(coloredImage.Width, coloredImage.Height);
			using (Graphics overlayGphx = Graphics.FromImage(grayscaleBitmap))
			{
				overlayGphx.SmoothingMode = SmoothingMode.AntiAlias;
				overlayGphx.DrawImage(coloredImage, new Rectangle(0, 0, coloredImage.Width, coloredImage.Height),
					0, 0, coloredImage.Width, coloredImage.Height, GraphicsUnit.Pixel, attributes);
			}

			return grayscaleBitmap;
		}

		public static unsafe Color GetDominantColor(Bitmap firstImage, Color containerBackColor)
		{
			// Shallow comparison to check same objects first
			if (firstImage == null)
				return containerBackColor;

			BitmapData firstImagePixels = firstImage.LockBits(new Rectangle(0, 0, firstImage.Width, firstImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			int iPixels = firstImage.Width * firstImage.Height;

			SortedDictionary<uint, int> histogram = new SortedDictionary<uint, int>();
			int tmp = 0;
			for (int iPixelIndex = 0; iPixelIndex < iPixels; iPixelIndex++)
			{
				if (histogram.ContainsKey(((uint*) firstImagePixels.Scan0)[iPixelIndex]))
					histogram[((uint*)firstImagePixels.Scan0)[iPixelIndex]]++;
				else histogram[((uint*)firstImagePixels.Scan0)[iPixelIndex]] = 1;
			}

			firstImage.UnlockBits(firstImagePixels);

			KeyValuePair<uint, int> maxItem = new KeyValuePair<uint, int>(0, -1);
			foreach (KeyValuePair<uint, int> keyValuePair in histogram)
			{
				if (keyValuePair.Value > maxItem.Value)
					maxItem = keyValuePair;
			}

			return maxItem.Key.Equals(0xFF000000) ? containerBackColor : Color.FromArgb(Convert.ToInt32(maxItem.Key));
		}

	}

	public class ZoomAndMargin
	{
		public int ZoomFactor { get; set; }
		public int Margin { get; set; }
	}
}
