namespace Nzy3d.Glut
{
	internal sealed class StrokeFontRec
	{
		public string name;
		public int num_chars;
		public StrokeCharRec[] ch;
		public float top;
		public float bottom;

		public StrokeFontRec(string name, int num_chars, StrokeCharRec[] ch, float top, float bottom)
		{
			this.name = name;
			this.num_chars = num_chars;
			this.ch = ch;
			this.top = top;
			this.bottom = bottom;
		}
	}
}
