namespace Nzy3d.Maths
{
	public sealed class IntegerCoord2d
	{
		public int X;

		public int Y;

		public IntegerCoord2d()
		{
			X = 0;
			Y = 0;
		}

		public IntegerCoord2d(int x, int y)
		{
			X = x;
			Y = y;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return "(IntegerCoord2d) x=" + X + " y=" + Y;
		}
	}
}
