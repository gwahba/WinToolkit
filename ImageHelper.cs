using System;
using System.Drawing;

namespace WinToolkit
{
	public static class ImageHelper
	{
		public static Image DrawText(String text, Font font, Color textColor, Color backColor)
		{
			//first, create a dummy bitmap just to get a graphics object
			Image img;
			Graphics drawing;
			SizeF textSize;
			using (img = new Bitmap(1, 1))
			using (drawing = Graphics.FromImage(img))
			{
				//measure the string to see how big the image needs to be
				textSize = drawing.MeasureString(text, font);
			}

			//create a new image of the right size
			img = new Bitmap((int)textSize.Width, (int)textSize.Height);
			using (drawing = Graphics.FromImage(img))
			using (Brush textBrush = new SolidBrush(textColor))
			{
				//paint the background
				drawing.Clear(backColor);
				drawing.DrawString(text, font, textBrush, 0, 0);
				drawing.Save();
			}
			return img;
		}

		public static Image DrawText(string text, Color textColor, Color backColor)
		{
			return DrawText(text, new Font("Segoe UI", 12, FontStyle.Bold), textColor, backColor);
		}
	}
}
