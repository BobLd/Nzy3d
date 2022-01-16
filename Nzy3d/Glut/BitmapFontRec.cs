namespace Nzy3d.Glut
{
	internal sealed class BitmapFontRec
	{
		public string name;
		public int num_chars;
		public int first;

		public BitmapCharRec[] ch;

		public BitmapFontRec(string name, int num_chars, int first, BitmapCharRec[] ch)
		{
			this.name = name;
			this.num_chars = num_chars;
			this.first = first;
			this.ch = ch;
		}
	}
}
