namespace Nzy3d.Glut
{
	public class BitmapCharRec
	{
		public int width;
		public int height;
		public float xorig;
		public float yorig;
		public float advance;

		public byte[] bitmap;

		public BitmapCharRec(int width, int height, float xorig, float yorig, float advance, byte[] bitmap)
		{
			this.width = width;
			this.height = height;
			this.xorig = xorig;
			this.yorig = yorig;
			this.advance = advance;
			this.bitmap = bitmap;
		}
	}
}
