﻿using Nzy3d.Plot3D.Builder;

namespace Nzy3d.Winforms.Plot3D.Builder.Concrete
{
    /// <summary>
    /// Mapper which reads height information from the grayscale
    /// values of a BufferedImage, normalized to range [0..1].
    /// </summary>
    public class BufferedImageMapper : Mapper
	{
		private Bitmap image;
		private int maxRow;

		private Rectangle maxViewPort;
		public BufferedImageMapper(Bitmap bi)
		{
			this.image = bi;
			this.maxRow = this.image.Height - 1;
			this.maxViewPort = new Rectangle(0, 0, bi.Width, bi.Height);
		}

		public Rectangle ClippedViewport(Rectangle roi)
		{
			return Rectangle.Intersect(this.maxViewPort, roi);
		}

		public override float f(float x, float y)
		{
			if (float.IsNaN(x) || float.IsNaN(y))
			{
				return float.NaN;
			}

			Color rgbColor = image.GetPixel(Convert.ToInt32(x), maxRow - Convert.ToInt32(y));
			return (rgbColor.R / 255 * 0.3f) + (rgbColor.G / 255 * 0.59f) + (rgbColor.B / 255 * 0.11f);
		}
	}
}
